# Api Documention
中南大学软件学院 技术专家考核 - 后端 API 文档

## 用户系统
使用 Http Basic Authorization.

### User/Login
进行登录操作, 返回用户基本信息, 请在调用其他 Api 前先调用本接口, 并在调用其他接口前传递正确的 Authorization.
#### 基本信息
请求类型: HTTP  
请求方式: GET  
响应类型: application/json 
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| UserName | string | 用户名 |
| Usage | int | 当前已存储文件大小, 单位 byte |
| Files | object(FileInfo) | 根目录信息 |
| -- Id | string | 根目录 ID |
| -- IsFolder | boolean | 是否为文件夹 |
| -- Metadata | object(FileMetadata) | 元信息 |
| ---- Name | string | 根目录名, 应为 ```root``` |
| ---- Size | int | 文件大小, 不适用于文件夹 |
| ---- UploadTime | dateTime | 文件夹建立时间 |
| -- ChildFiles | array(FileInfo) | 子文件夹的文件列表 |
| ---- Id | string | 文件夹 ID |
| ---- IsFolder | boolean | 是否为文件夹 |
| ---- Metadata | object(FileMetaData) | 元信息 |
| ------ Name | string | 文件夹名 |
| ------ Size | int | 文件大小, 不适用于文件夹 |
| ------ UploadTime | dateTime | 文件夹建立时间 |
| ---- ChildFiles | array(FileInfo) | 应为空, 子文件列表 |
#### 请参阅
[Add support for basic auth by zcbenz · Pull Request #3250 · electron/electron](https://github.com/electron/electron/pull/3250) 

### User/Register
用户注册接口, 请使用 HTTP Basic Authorization 将用户名和密码传递至该接口.
#### 基本信息
请求类型: HTTP  
请求方式: GET  
响应类型: application/json 
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| UserName | string | 用户名 |
| Usage | int | 当前已存储文件大小, 单位 byte |
| Files | object(FileInfo) | 根目录信息 |
| -- Id | string | 根目录 ID |
| -- IsFolder | boolean | 是否为文件夹 |
| -- Metadata | object(FileMetadata) | 元信息 |
| ---- Name | string | 根目录名, 应为 ```root``` |
| ---- Size | int | 文件大小, 不适用于文件夹 |
| ---- UploadTime | dateTime | 文件夹建立时间 |
| -- ChildFiles | array(FileInfo) | 子文件夹的文件列表 |
| ---- Id | string | 文件夹 ID |
| ---- IsFolder | boolean | 是否为文件夹 |
| ---- Metadata | object(FileMetaData) | 元信息 |
| ------ Name | string | 文件夹名 |
| ------ Size | int | 文件大小, 不适用于文件夹 |
| ------ UploadTime | dateTime | 文件夹建立时间 |
| ---- ChildFiles | array(FileInfo) | 应为空, 子文件列表 |

## 文件系统
### File/GetFolder
获取某一文件夹下所有文件和文件夹的信息.
#### 基本信息
请求类型: HTTP   
请求方式: GET  
响应类型: application/json
#### 请求数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| folderId | string | 文件 ID, 要求为文件夹, 为空则返回根目录 |
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| Id | string | 文件夹 ID |
| IsFolder | boolean | 是否为文件夹 |
| Metadata | object(FileMetadata) | 元信息 |
| -- Name | string | 文件夹名 |
| -- Size | int | 文件大小, 不适用于文件夹 |
| -- UploadTime | DateTime | 文件夹建立时间 |
| ChildFiles | array(FileInfo) | 当前文件夹的文件列表 |
| -- Id | string | 文件夹 ID |
| -- IsFolder | boolean | 是否为文件夹 |
| -- Metadata | object(FileMetadata) | 元信息 |
| ---- Name | string | 文件夹名 |
| ---- Size | int | 文件大小, 不适用于文件夹 |
| ---- UploadTime | DateTime | 文件夹建立时间 |
| ---- ChildFiles | array(FileInfo) | 应为空, 子文件列表 |

### File/Upload
上传文件.
#### 基本信息
请求类型: multipart/form-data   
请求方式: POST  
响应类型: application/json  
#### 请求数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| objectId | string | 目标文件ID, 要求为文件夹, 为空则上传到根目录 |
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| Id | string | 文件夹 ID |
| IsFolder | boolean | 是否为文件夹 |
| Metadata | object(FileMetadata) | 元信息 |
| -- Name | string | 文件夹名 |
| -- Size | int | 文件大小, 不适用于文件夹 |
| -- UploadTime | DateTime | 文件夹建立时间 |
| ChildFiles | array(FileInfo) | 当前文件夹的文件列表 |
| -- Id | string | 文件夹 ID |
| -- IsFolder | boolean | 是否为文件夹 |
| -- Metadata | object(FileMetadata) | 元信息 |
| ---- Name | string | 文件夹名 |
| ---- Size | int | 文件大小, 不适用于文件夹 |
| ---- UploadTime | DateTime | 文件夹建立时间 |
| ---- ChildFiles | array(FileInfo) | 应为空, 子文件列表 |
#### 说明
目前支持最大 2G 的单文件上传.
#### 请参阅
[组件 | Element](http://element.eleme.io/#/zh-CN/component/upload)  
[Forms in HTML documents](https://www.w3.org/TR/html401/interact/forms.html#h-17.13.4.2)
[Uploading Files](https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/uploadingfiles/)

### File/Download
下载文件, 获得文件流.
#### 基本信息
请求类型: HTTP   
请求方式: POST  
响应类型: application/octet-stream 
#### 请求参数
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| objectId | string | 文件ID, 要求为文件 |
#### 响应数据
一个二进制文件流.
#### 请参阅
[ASP.NET(C#) Web Api通过文件流下载文件到本地实例](http://2sharings.com/2015/dot-net-download-file-from-web-api)  
[electron/web-contents.md at master · electron/electron](https://github.com/electron/electron/blob/master/docs/api/web-contents.md)  
[Downloads API · Issue #2491 · electron/electron](https://github.com/electron/electron/issues/2491)  
[request/request: Simplified HTTP request client.](https://github.com/request/request#streaming)  
[Downloading Files](https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/downloadingfiles/)

### File/Rename
重命名文件, 可以重命名文件或文件夹.
#### 基本信息
请求类型: HTTP  
请求方式: POST  
响应类型: application/json
#### 请求参数
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| objectId | string | 要重命名的文件ID |
| fileName | string | 新文件名 |
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| Id | string | 文件夹 ID |
| IsFolder | boolean | 是否为文件夹 |
| Metadata | object(FileMetadata) | 元信息 |
| -- Name | string | 文件夹名 |
| -- Size | int | 文件大小, 不适用于文件夹 |
| -- UploadTime | DateTime | 文件夹建立时间 |
| ChildFiles | array(FileInfo) | 当前文件夹的文件列表 |
| -- Id | string | 文件夹 ID |
| -- IsFolder | boolean | 是否为文件夹 |
| -- Metadata | object(FileMetadata) | 元信息 |
| ---- Name | string | 文件夹名 |
| ---- Size | int | 文件大小, 不适用于文件夹 |
| ---- UploadTime | DateTime | 文件夹建立时间 |
| ---- ChildFiles | array(FileInfo) | 应为空, 子文件列表 |
#### 请参阅
[Deleting and Renaming Files](https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/deletingandrenamingfiles/)

### File/Delete
删除文件, 可以删除文件或文件夹.
#### 基本信息
请求类型: HTTP  
请求方式: POST  
响应类型: application/json
#### 请求参数
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| objectId | string | 要删除的文件ID |
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| Id | string | 文件夹 ID |
| IsFolder | boolean | 是否为文件夹 |
| Metadata | object(FileMetadata) | 元信息 |
| -- Name | string | 文件夹名 |
| -- Size | int | 文件大小, 不适用于文件夹 |
| -- UploadTime | DateTime | 文件夹建立时间 |
| ChildFiles | array(FileInfo) | 当前文件夹的文件列表 |
| -- Id | string | 文件夹 ID |
| -- IsFolder | boolean | 是否为文件夹 |
| -- Metadata | object(FileMetadata) | 元信息 |
| ---- Name | string | 文件夹名 |
| ---- Size | int | 文件大小, 不适用于文件夹 |
| ---- UploadTime | DateTime | 文件夹建立时间 |
| ---- ChildFiles | array(FileInfo) | 应为空, 子文件列表 |
#### 请参阅
[Deleting and Renaming Files](https://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/deletingandrenamingfiles/)

### File/Move
移动文件, 可以移动文件或文件夹.
#### 基本信息
请求类型: HTTP  
请求方式: POST  
响应类型: application/json
#### 请求参数
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| objectId | string | 要移动的文件ID |
| targetId | string | 要移动到的文件夹, 要求为文件夹 |
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| fileList | array(FileInfo) | 目标文件夹的文件列表 |
| -name | string | 文件名 |
| -objectId | string | 文件ID |
| -isFolder | boolean | 是否为文件夹 |
| -size | int | 文件大小 |
| -uploadTime | string | 上传时间 | 

### File/Copy
复制文件, 可以复制文件或文件夹.
#### 基本信息
请求类型: HTTP  
请求方式: POST  
响应类型: application/json
#### 请求参数
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| objectId | string | 要复制的文件ID |
| targetId | string | 要复制到的文件夹, 要求为文件夹 |
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| fileList | array(FileInfo) | 目标文件夹的文件列表 |
| -name | string | 文件名 |
| -objectId | string | 文件ID |
| -isFolder | boolean | 是否为文件夹 |
| -size | int | 文件大小 |
| -uploadTime | string | 上传时间 | 


### File/NewFolder
创建新文件夹.
#### 基本信息
请求类型: HTTP  
请求方式: POST  
响应类型: application/json
#### 请求参数
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| objectId | string | 母文件ID, 要求为文件夹, 为空则在根目录创建 |
| folderName | string | 文件夹名 |
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| fileList | array(FileInfo) | 当前文件夹的文件列表 |
| -name | string | 文件名 |
| -objectId | string | 文件ID |
| -isFolder | boolean | 是否为文件夹 |
| -size | int | 文件大小 |
| -uploadTime | string | 上传时间 | 

### File/Search
按指定关键词搜索文件.
#### 基本信息
请求类型: HTTP  
请求方式: POST  
响应类型: application/json
#### 请求参数
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| keyWords | string | 关键词 |
| limit | int | 搜索结果数, 默认为100 |
| skip | int | 跳过前若干个结果, 默认为0, 用于实现分页 |
#### 响应数据
| 参数名称 | 类型 | 描述 |
| ------- | ---- | --- |
| fileList | array(FileInfo) | 符合要求的文件列表 |
| -name | string | 文件名 |
| -objectId | string | 文件ID |
| -isFolder | boolean | 是否为文件夹 |
| -parentId | string | 所在文件夹ID |
| -parentName | string | 所在文件夹名称 |
| -size | int | 文件大小 |
| -uploadTime | string | 上传时间 | 