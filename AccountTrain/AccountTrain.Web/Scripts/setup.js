///////Vue 校验的初始化与扩展验证
VeeValidate.Validator.localize("zh_CN");
VeeValidate.Validator.extend("phone", {
    getMessage: function () {
        return "请输入正确的手机号"
    },
    validate: function (val) {
        return /^1\d{10}$/.test(val)
    }
})

Vue.use(VeeValidate); //一般插件都要use一下