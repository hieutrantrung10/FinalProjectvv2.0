var email= {
    init:function() {
        email.registerEvents();
    },
    registerEvents: function() {
        $('#btnSend').off('click').on('click', function() {
            var email = $('#txtEmail').val();
            window.alert('Thu cua ban da duoc gui');
            $.ajax({
                url: '/User/ResendEmail',
                type: 'POST',
                dataType: 'json',
                data: {
                    email: email
                },
                success: function(res) {
                    if (res.status == true) {
                        //window.alert('Cảm ơn bạn đã gửi phản hồi cho chúng tôi!!');
                        contact.resetForm();
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