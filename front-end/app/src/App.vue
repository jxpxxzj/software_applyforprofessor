<style>
/* Global Style Sheet */
* {
    /* from Element-UI */
    font-family: "Helvetica Neue",Helvetica,"PingFang SC","Hiragino Sans GB","Microsoft YaHei","微软雅黑",Arial,sans-serif;
}
html {
    height: 100%;
}
body {
    height: 100%;
}
#app {
    height: 100%;
}
</style>
<script>
// Prevent zooming by scroll
// Ref: https://github.com/electron/electron/issues/2500
window.addEventListener('mousewheel', (e) => {
    if (e.ctrlKey) {
        e.preventDefault();
    }
});

</script>