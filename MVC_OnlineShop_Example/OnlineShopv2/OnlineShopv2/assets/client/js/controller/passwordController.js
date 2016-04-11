var email = {
    init: function () {
        email.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var email = $('#txtEmail').val();
            var username = $('#txtUserName').val();
            window.alert('Thư của bạn đã được gửi');
            $.ajax({
                url: '/User/ForgotPassword',
                type: 'POST',
                dataType: 'json',
                data: {
                    email: email,
                    username:username
                },
                success: function (res) {
                    if (res.status == true) {
                        email.resetForm();
                    }
                }
            });
        });
    },
    resetForm: function () {
        $('txtEmail').val('');
    }
}
email.init();