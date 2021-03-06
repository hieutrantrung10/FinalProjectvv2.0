﻿var email= {
    init:function() {
        email.registerEvents();
    },
    registerEvents: function() {
        $('#btnSend').off('click').on('click', function() {
            var email = $('#txtEmail').val();
            window.alert('Thư của bạn đã được gửi');
            $.ajax({
                url: '/User/ResendEmail',
                type: 'POST',
                dataType: 'json',
                data: {
                    email: email
                },
                success: function(res) {
                    if (res.status == true) {
                        email.resetForm();
                    }
                }
            });
        });
    },
    resetForm:function() {
        $('txtEmail').val('');
    }
}
email.init();