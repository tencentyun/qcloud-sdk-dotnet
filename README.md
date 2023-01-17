# qcloud-sdk-dotnet

![Github](https://img.shields.io/github/release/tencentyun/qcloud-sdk-dotnet.svg) ![TravisCI](https://travis-ci.org/tencentyun/qcloud-sdk-dotnet.svg?branch=master)

腾讯云 COS XML .NET SDK 发布仓库

## 添加 SDK

我们提供 Nuget 的集成方式，您可以在工程的 csproj 文件里添加：

```
<PackageReference Include="Tencent.QCloud.Cos.Sdk" Version="5.4.*" />
```

如果是用 .Net CLI，请使用如下命令安装：

```
dotnet add package Tencent.QCloud.Cos.Sdk
```

如果是用于目标框架 .Net Framework 4.0 及以下版本的开发，

请使用 [Releases](https://github.com/tencentyun/qcloud-sdk-dotnet/releases) 下载的COSXML-Compatible.dll文件

在Visual Studio项目中选择 

```
项目 -> 添加引用 -> 浏览 -> COSXML-Compatible.dll 的方式添加C# SDK
```

您也可以在 [Releases 里面](https://github.com/tencentyun/qcloud-sdk-dotnet/releases) 手动下载我们的SDK。

## 开发文档

如果您想要查看每个 API, SDK 是如何调用的，请参考 [腾讯云官网文档](https://cloud.tencent.com/document/product/436/32819)。

## License

腾讯云 .NET SDK 通过 `MIT ` License 发布。

```shell
Copyright (c) 2018 腾讯云

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
