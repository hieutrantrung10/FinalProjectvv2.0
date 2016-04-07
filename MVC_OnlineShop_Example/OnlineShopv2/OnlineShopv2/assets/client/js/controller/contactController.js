var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var name = $('#txtName').val();
            var mobile = $('#txtMobile').val();
            var address = $('#txtAddress').val();
            var email = $('#txtEmail').val();
            var content = $('#txtContent').val();
            window.alert('Cảm ơn bạn đã gửi phản hồi với chúng tôi (ง ͠° ͟ل͜ ͡°)ง');
            $.ajax({
                url: '/Contact/Send',
                type: 'POST',
                dataType: 'json',
                data: {
                    name: name,
                    mobile: mobile,
                    address: address,
                    email: email,
                    content: content
                },
                success: function (res) {
                    if (res.status == true) {
                        //window.alert('Cảm ơn bạn đã gửi phản hồi cho chúng tôi!!');
                        contact.resetForm();
                    }
                }
            });
        });
    },
    resetForm: function () {
        $('#txtName').val('');
        $('#txtMobile').val('');
        $('#txtAddress').val('');
        $('#txtEmail').val('');
        $('#txtContent').val('');
    }
}
contact.init();