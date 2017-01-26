<template>
    <div class="home-container">
        <el-row class="home-toolbar-container">
            <el-col :span="16">
                <el-button type="primary" @click="uploadVisible = true">上传<i class="el-icon-upload el-icon--right"></i></el-button>
                <el-button @click="openCreateFolder" v-if="!inSearch">新建文件夹</el-button>
                <el-button @click="fetchData" v-if="!inSearch">刷新</el-button>
                <el-button @click="openMenu">更多</el-button>
            </el-col>
            <el-col :span="8">
                <el-input placeholder="请输入关键词" v-model="keywords">
                    <el-button slot="append" icon="search" @click="handleSearch"></el-button>
                </el-input>
            </el-col>
        </el-row>
        <el-row class="home-path-container">
            <span>
                <span v-if="parent != '000000000000000000000000'">
                    <el-button size="small" @click="folderClick(parent)">向上</el-button>
                    <el-button size="small" @click="folderClick(rootFolder)">根目录</el-button>
                </span>
                <span v-else>
                    <el-button size="small" :disabled="true">向上</el-button>
                </span>
            </span>
            &nbsp;&nbsp;{{ currentFolder }}
        </el-row>          
        <el-row class="home-data-container">
            <el-table :data="fileData" border style="width: 100%" v-loading.body="fileLoading" element-loading-text="拼命加载中" :default-sort = "{prop: 'IsFolder', order: 'descending'}">
                <el-table-column sortable label="文件名" width="120">
                    <template scope="scope">
                        <el-icon name="document" v-if="!scope.row.IsFolder"></el-icon>
                        <el-button type="text" v-if="scope.row.IsFolder" @click="folderClick(scope.row.Id)">{{ scope.row.Metadata.Name }}</el-button>
                        <div v-else>{{ scope.row.Metadata.Name }}</div>
                    </template>
                </el-table-column>
                <el-table-column sortable prop="IsFolder" label="大小" width="120">
                    <template scope="scope">
                        <div v-if="scope.row.IsFolder">
                            -
                        </div>
                        <div v-else>
                            {{ scope.row.Metadata.Size / 1000 }} KiB
                        </div>
                    </template>
                </el-table-column>
                <el-table-column label="修改时间">
                     <template scope="scope">
                        {{ scope.row.Metadata.UploadTime.replace('T',' ').replace('Z','') }}
                    </template>
                </el-table-column>
                <el-table-column label="操作">
                    <template scope="scope">
                        <el-button v-if="!scope.row.IsFolder" size="small" type="primary" @click="handleDownload(scope.$index, scope.row)">下载</el-button>
                        <el-button size="small" @click="handleRename(scope.$index,scope.row)">重命名</el-button>
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

        <el-dialog title="关于" v-model="aboutVisible" size="medium">
            <h2 class="home-about-title">Webdisk</h2>
            Version {{ this.$electron.remote.app.getVersion() }}<br>
            Shell {{ process.versions['atom-shell'] }}<br>
            Renderer {{ process.versions.chrome }}<br>
            Node {{ process.versions.node }}<br>
        </el-dialog>

        <el-dialog title="设置" v-model="settingsVisible" size="full" v-on:open="onSettingsOpen" v-on:close="onSettingsClose">
            <el-tabs>
                <el-tab-pane label="下载" name="first">
                    <el-row :gutter="20">
                        <el-col :span="15">
                            <el-input v-model="downloadPath">
                                <template slot="prepend">下载路径</template>
                            </el-input>
                        </el-col>
                        <el-col :span="9">
                            <el-button @click="settings_select">浏览...</el-button>
                            <el-button @click="settings_defaultPath">默认路径</el-button>
                        </el-col>
                    </el-row>
                </el-tab-pane>
                <el-tab-pane label="通知" name="second">
                    下载或上传完成时发送通知&nbsp; <el-switch v-model="enableNotification" @change="settings_notification"></el-switch>
                </el-tab-pane>
            </el-tabs>
        </el-dialog>
    </div>
</template>
<style scoped>
.home-toolbar-container
{
    padding-top: 15px;
    padding-bottom: 15px;
    padding-left: 10px;
    padding-right: 10px;
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
    width: 100%;
    position: fixed;
    z-index: 999;
    background-color: #f9fafc;
}
.home-data-container
{
    padding-top:110px;
}
.home-about-title
{
    color:#1d8ce0;
}
</style>
<script>
export default {
    data () {
        return {
            fileData: [],
            currentFolder: '',
            parent: '',
            keywords: '',
            uploadVisible: false,
            aboutVisible: false,
            settingsVisible:false,
            folder:'14d1908575be46c2bc4e01c2',
            rootFolder:'14d1908575be46c2bc4e01c2',
            inSearch: false,
            fileLoading: true,
            menu: null,
            process: window.process,
            downloadPath: '',
            enableNotification: false
        }
    },
    created() {
        this.fetchData(this.folder);
        const remote = this.$electron.remote;
        const Menu = remote.Menu;
        const MenuItem = remote.MenuItem;
        this.menu = new Menu();
        const vm = this;
        this.menu.append(new MenuItem({ label: '设置', click() { 
                vm.settingsVisible = true;
            } 
        }));
        this.menu.append(new MenuItem({ label: '显示开发者工具', click() { 
                remote.getCurrentWindow().openDevTools();
            } 
        }));
        this.menu.append(new MenuItem({ type: 'separator' }));
        this.menu.append(new MenuItem({ label: '关于', click() { 
                vm.aboutVisible = true;
            } 
        }));
    },
    methods: {
        fetchData(fo = this.folder) { 
            this.fileLoading = true;
            this.$http.get("http://localhost:7308/api/File/GetFolder?objectId=" + fo)
            .then((response) => {
                const result = response.json().then((value) => {
                    this.inSearch = false;
                    this.fileData = value.ChildFiles;
                    this.currentFolder = value.Metadata.Name;
                    this.parent = value.Parent;
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
                this.$http.get('http://localhost:7308/api/File/CreateFolder?objectId=' + this.folder + '&name=' + value)
                .then((response) => {
                    this.$message({
                        type: 'success',
                        message: '创建成功.'
                    });
                    this.fetchData();        
                })
            })
            .catch(() => {

            });
        },
        upload_onprogress(event, file, fileList) {
            this.$electron.remote.getCurrentWindow().setProgressBar(event.percent / 100);        
        },
        upload_onsuccess (response, file, fileList) {
            this.$electron.remote.getCurrentWindow().setProgressBar(0);  
            if(this.$store.state.settings.enableNotification) {
                const n = new window.Notification('Webdisk', { body: '上传完成' });
                n.onclick = () => {
                    
                };
            }
            this.fetchData();
        },
        handleDownload(index,row) {

        },
        handleRename(index,row) {
            this.$prompt('请输入新文件名', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                inputPlaceholder: row.Metadata.Name
                })
            .then(({ value }) => {
                this.$http.get('http://localhost:7308/api/File/Rename?objectId=' + row.Id + '&newFilename=' + value)
                .then((response) => {
                    this.$message({
                        type: 'success',
                        message: '重命名成功.'
                    });
                    this.fetchData();        
                })
            })
            .catch(() => {

            });
        },
        handleDelete(index, row) {
            this.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            })
            .then(() => {
                this.$http.get('http://localhost:7308/api/File/Delete?objectId=' + row.Id)
                .then((response) => {
                    this.$message({
                        type: 'success',
                        showClose: true,
                        duration: 1500,
                        message: '删除成功!'
                    });
                    this.fetchData();        
                }) 
            })
            .catch(() => {

            });
        },
        folderClick(id) {
            this.parent = this.folder;
            this.folder = id;
            this.fetchData();
        },
        openMenu() {
            this.menu.popup(this.$electron.remote.getCurrentWindow(),293,51);
        },  
        onSettingsClose() {
            this.$store.commit('saveSettings');
            this.$message({
                type: 'success',
                showClose: true,
                duration: 1500,
                message: '设置已保存.'
            });
        },
        onSettingsOpen() {
            this.$store.commit('initSettings');
            this.downloadPath = this.$store.state.settings.downloadPath;
            this.enableNotification = this.$store.state.settings.enableNotification;
        },
        settings_select() {
            this.$electron.remote.dialog.showOpenDialog({
                properties: ['openDirectory']
            },(filename) => {
                if (filename === undefined) {
                    return;
                }
                this.downloadPath = filename[0];
                this.$store.commit('setDownloadPath',this.downloadPath);
            });
        },
        settings_defaultPath() {
            this.downloadPath = this.$electron.remote.app.getPath('downloads');
            this.$store.commit('setDownloadPath',this.downloadPath);
        },
        settings_notification(value) {
            this.enableNotification = value;
            this.$store.commit('setEnableNotification',this.enableNotification);
        },
        handleSearch() {
            this.fileLoading = true;
            this.$http.get('http://localhost:7308/api/File/Search?keywords=' + this.keywords)
            .then((response) => {
                const result = response.json().then((value) => {
                    this.inSearch = true;
                    this.fileData = value;
                    this.parent = this.rootFolder;
                    this.currentFolder = '搜索: ' + this.keywords;
                });
                this.fileLoading = false;
            })
            .catch(() => {

            });
        }
    }
}
</script>