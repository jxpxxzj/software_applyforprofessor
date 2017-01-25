<template>
    <div class="home-container">
        <el-row class="home-toolbar-container">
            <el-col :span="16">
                <el-button type="primary" @click="uploadVisible = true">上传<i class="el-icon-upload el-icon--right"></i></el-button>
                <el-button @click="openCreateFolder">新建文件夹</el-button>
                <el-button @click="fetchData">刷新</el-button>
            </el-col>
            <el-col :span="8">
                <el-input placeholder="请输入关键词" v-model="keywords">
                    <el-button slot="append" icon="search"></el-button>
                </el-input>
            </el-col>
        </el-row>
        <el-row class="home-path-container">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item>网盘</el-breadcrumb-item>
                <el-breadcrumb-item>root</el-breadcrumb-item>
            </el-breadcrumb>      
        </el-row>          
        <el-row class="home-data-container">
            <el-table :data="fileData" border style="width: 100%" v-loading.body="fileLoading" element-loading-text="拼命加载中" :default-sort = "{prop: 'IsFolder', order: 'descending'}">
                <el-table-column sortable label="文件名" width="120">
                    <template scope="scope">
                        <el-icon name="document" v-if="!scope.row.IsFolder"></el-icon>
                        {{ scope.row.Metadata.Name }}
                    </template>
                </el-table-column>
                <el-table-column sortable label="大小" width="120">
                    <template scope="scope">
                        <div v-if="scope.row.IsFolder">
                            -
                        </div>
                        <div v-else>
                            {{ scope.row.Metadata.Size / 1000 }} KiB
                        </div>
                    </template>
                </el-table-column>
                <el-table-column sortable label="修改时间">
                     <template scope="scope">
                        {{ scope.row.Metadata.UploadTime.replace('T',' ').replace('Z','') }}
                    </template>
                </el-table-column>
                <el-table-column label="操作">
                    <template scope="scope">
                        <el-button v-if="!scope.row.IsFolder" size="small" type="primary" @click="handleDownload(scope.$index, scope.row)">下载</el-button>
                        <el-button size="small" type="danger" @click="handleDelete(scope.$index, scope.row)">删除</el-button>
                    </template>
                </el-table-column>
            </el-table>
        </el-row>
        <el-dialog title="文件上传" v-model="uploadVisible" size="medium">
            <el-upload
                action="http://localhost:7308/api/File/Upload"
                :on-progress="upload_onprogress"
                :on-success="upload_onsuccess"
                :multiple=true
                type="drag"
                :data="{ folder: this.folder }"
                :headers="{ Authorization: 'Basic UGFycnk6MTIzNDU2'}"
            >
                <i class="el-icon-upload"></i>
                <div class="el-dragger__text">将文件拖到此处，或<em>点击上传</em></div>
            </el-upload>
        </el-dialog>
    </div>
</template>
<style scoped>
.home-toolbar-container
{
    padding-top: 15px;
    padding-bottom: 15px;
    padding-left: 10px;
    position: fixed;
    z-index: 999;
    background-color: #f9fafc;
}
.home-path-container
{
    padding-left: 10px;
    margin-top: 66px;
    height: 20px;
    min-height: 20px;
    text-align: center;
    width: 100%;
    position: fixed;
    z-index: 999;
    background-color: #f9fafc;
}
.home-data-container
{
    padding-top:110px;
}
</style>
<script>
export default {
    data () {
        return {
            fileData: [],
            keywords: '',
            uploadVisible: false,
            folder:'14d1908575be46c2bc4e01c2',
            fileLoading: true
        }
    },
    created() {
        this.fetchData();
    },
    methods: {
        fetchData() { 
            this.fileLoading = true;
            this.$http.get("http://localhost:7308/api/File/GetFolder?")
            .then((response) => {
                const result = response.json().then((value) => {
                    this.fileData = value.ChildFiles;
                });
                this.fileLoading = false;
            });
        },
        openCreateFolder() {
            this.$prompt('请输入文件夹名', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                inputPattern: /^[^\\\\\\/:*?\\"<>|]+$/,
                inputErrorMessage: '文件夹名格式不正确'
                })
            .then(({ value }) => {
                this.$http.get("http://localhost:7308/api/File/CreateFolder?objectId=root&name=" + value )
                .then((response) => {
                    this.$message({
                        type: 'success',
                        message: '创建成功.'
                    });
                    this.fetchData();        
                })
                
            });
        },
        upload_onprogress(event, file, fileList) {
            this.$electron.remote.getCurrentWindow().setProgressBar(event.percent / 100);        
        },
        upload_onsuccess (response, file, fileList) {
            console.log(response);
            this.$electron.remote.getCurrentWindow().setProgressBar(0);  
            const n = new window.Notification('Webdisk', { body: '上传完成' });
            n.onclick = () => {
                
            };
            this.fetchData();
        }
    }
}
</script>