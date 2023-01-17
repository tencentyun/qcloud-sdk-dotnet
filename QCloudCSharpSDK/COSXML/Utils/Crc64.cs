using System;
using System.IO;

namespace COSXML.Utils
{
    public class Crc64
    {
        private static ulong[] _table;
        private static object _lock = new object();
        private const int GF2_DIM = 64;
        private static ulong _poly;

        private static void GenStdCrcTable(ulong poly)
        {
            _poly = poly;

            _table = new ulong[256];

            for (uint n = 0; n < 256; n++)
            {
                ulong crc = n;
                for (int k = 0; k < 8; k++)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = (crc >> 1) ^ poly;
                    }
                    else
                    {
                        crc = (crc >> 1);
                    }
                }

                _table[n] = crc;
            }
        }

        private static ulong TableValue(ulong[] table, byte b, ulong crc)
        {
            unchecked
            {
                return (crc >> 8) ^ table[(crc ^ b) & 0xffUL];
            }
        }

        public static void Init(ulong poly)
        {
            if (_table == null)
            {
                lock (_lock)
                {
                    if (_table == null)
                    {
                        GenStdCrcTable(poly);
                    }
                }
            }
        }

        public static void InitECMA()
        {
            Init(0xC96C5795D7870F42);
        }

        public static ulong Compute(byte[] bytes, int start, int size, ulong crc = 0)
        {
            crc = ~crc;
            for (var i = start; i < start + size; i++)
            {
                crc = TableValue(_table, bytes[i], crc);
            }

            crc = ~crc;
            return crc;
        }

        private static ulong Gf2MatrixTimes(ulong[] mat, ulong vec)
        {
            ulong sum = 0;
            int idx = 0;
            while (vec != 0)
            {
                if ((vec & 1) == 1)
                {
                    sum ^= mat[idx];
                }

                vec >>= 1;
                idx++;
            }
            
            return sum;
        }

        private static void Gf2MatrixSquare(ulong[] square, ulong[] mat)
        {
            for (int n = 0; n < GF2_DIM; n++) 
            {
                square[n] = Gf2MatrixTimes(mat, mat[n]);
            }
        }

        static public ulong Combine(ulong crc1, ulong crc2, long len2)
        {
            // degenerate case.
            if (len2 == 0)
            {
                return crc1;
            }

            int n;
            ulong row;
            ulong[] even = new ulong[GF2_DIM]; 
            ulong[] odd = new ulong[GF2_DIM]; 

            // put operator for one zero bit in odd
            odd[0] = _poly;

            row = 1;
            for (n = 1; n < GF2_DIM; n++)
            {
                odd[n] = row;
                row <<= 1;
            }

            // put operator for two zero bits in even
            Gf2MatrixSquare(even, odd);

            // put operator for four zero bits in odd
            Gf2MatrixSquare(odd, even);

            // apply len2 zeros to crc1 (first square will put the operator for one
            // zero byte, eight zero bits, in even)
            do
            {
                // apply zeros operator for this bit of len2
                Gf2MatrixSquare(even, odd);
                if ((len2 & 1) == 1)
                {
                    crc1 = Gf2MatrixTimes(even, crc1);
                }
                    
                len2 >>= 1;

                // if no more bits set, then done
                if (len2 == 0)
                {
                    break;
                }

                // another iteration of the loop with odd and even swapped
                Gf2MatrixSquare(odd, even);
                if ((len2 & 1) == 1)
                {
                    crc1 = Gf2MatrixTimes(odd, crc1);
                }

                len2 >>= 1;

                // if no more bits set, then done
            } while (len2 != 0);

            // return combined crc.
            crc1 ^= crc2;
            return crc1;
        }

        public static bool CompareCrc64(string localFile, string crc64ecma)
        {
            Crc64.InitECMA();
            String hash = String.Empty;

            using (FileStream fs = File.Open(localFile, FileMode.Open))
            {
                byte[] buffer = new byte[2048];
                int bytesRead;
                ulong crc = 0;

                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0) 
                {
                    ulong partCrc = Crc64.Compute(buffer, 0, bytesRead);
                    if (crc == 0) 
                    {
                        crc = partCrc;
                    }
                    else 
                    {
                        crc = Crc64.Combine(crc, partCrc, bytesRead);
                    }
                }

                string localFileCrc64 = crc.ToString();
                return localFileCrc64 == crc64ecma;
            }
        }
    }
}