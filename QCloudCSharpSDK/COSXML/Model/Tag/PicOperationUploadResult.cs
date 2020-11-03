using System.Collections.Generic;

namespace COSXML.Model.Tag {
  /// <summary>
  /// 图片处理结果
  /// </summary>
  public sealed class PicOperationUploadResult {

    /// <summary>
    /// 原图信息
    /// </summary>
    public OriginalInfo originalInfo;

    /// <summary>
    /// 图片处理结果
    /// </summary>
    public ProcessResults processResults;

    /// <summary>
    /// 原图信息
    /// </summary>
    public sealed class OriginalInfo {
      /// <summary>
      /// 对象健
      /// </summary>
      public string Key;
      /// <summary>
      /// 图片路径
      /// </summary>
      public string Location;
      /// <summary>
      /// 图片 Etag
      /// </summary>
      public string ETag;
      /// <summary>
      /// 原图图片信息
      /// </summary>
      public ImageInfo imageInfo;
    }

    /// <summary>
    /// 原图图片信息
    /// </summary>
    public sealed class ImageInfo {
      /// <summary>
      /// 格式
      /// </summary>
      public string Format;
      /// <summary>
      /// 宽度
      /// </summary>
      public int Width;
      /// <summary>
      /// 高度
      /// </summary>
      public int Height;
      /// <summary>
      /// 图片质量
      /// </summary>
      public int Quality;
      /// <summary>
      /// 图片主色调
      /// </summary>
      public string Ave;
      /// <summary>
      /// 图片旋转角度
      /// </summary>
      public int Orientation;
    }

    /// <summary>
    /// 图片处理结果
    /// </summary>
    public sealed class ProcessResults {
      /// <summary>
      /// 图片处理结果
      /// </summary>
      public List<ProcessResult> results;

      public ProcessResults() {
        results = new List<ProcessResult>();
      }
    }

    /// <summary>
    /// 单个图片处理结果
    /// </summary>
    public sealed class ProcessResult {
      /// <summary>
      /// 文件对象键
      /// </summary>
      public string Key;
      /// <summary>
      /// 图片路径
      /// </summary>
      public string Location;
      /// <summary>
      /// 图片格式
      /// </summary>
      public string Format;
      /// <summary>
      /// 图片宽度
      /// </summary>
      public int Width;
      /// <summary>
      /// 图片高度
      /// </summary>
      public int Height;
      /// <summary>
      /// 图片大小
      /// </summary>
      public int Size;
      /// <summary>
      /// 图片质量
      /// </summary>
      public int Quality;
      /// <summary>
      /// 图片 Etag
      /// </summary>
      public string ETag;
      /// <summary>
      /// 盲水印时表示提取到全盲水印的可信度
      /// </summary>
      public int WatermarkStatus;
    }
  }
}