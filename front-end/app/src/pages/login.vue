<template>
    <div class="login-background">
        <div class="login-container">
            <el-row type="flex" justify="center">
                <img src="../assets/cloud_disk.png" class="login-logo">
            </el-row>
            <el-row>
                <el-input size="large" class="login-input-username" placeholder="Parry" :disabled=true>
                    <template slot="prepend">用户名</template>
                </el-input>
            </el-row>
            <el-row>&nbsp;</el-row>
            <el-row>
                <el-input size="large" class="login-input-password" type="password" placeholder="123456" :disabled=true>
                    <template slot="prepend">密码</template>
                 </el-input>
            </el-row>
            <el-row>&nbsp;</el-row>
            <el-row type="flex" justify="space-around">
                <el-button size="large":disabled=true>注册</el-button>
                <el-button size="large" type="primary" @click="login">登录</el-button>
            </el-row>
        </div>
    </div>
</template>
<script>
export default {
    methods: {
        login() {
            this.$router.push({ path: '/home'});
        }
    }
}
</script>
<style scoped>
body
{
    display: flex;
    align-items: center;
    justify-content: center;
    height: 100%;
    margin: auto;
}
#app
{
    height: 100%;
    text-align: center;
    width: 100%;
}
.login-background
{
    height: 100%;
    background-image: url(../assets/login-bg.jpg);
    background-repeat: no-repeat;
    background-size: cover;
}
.login-container
{
    margin: auto;
    max-width: 600px;
    position: absolute;  
    top: 0; left: 0; bottom: 0; right: 0; 
    height: auto;
}
.login-logo 
{
    width: auto;
    height: 150px;
}
</style>