var order = {
    init:function() {
        order.registerEvents();
    },
    registerEvents: function() {
        $('.btn-change').off('click').on('click', function(e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data("id");
            $.ajax({
                url: "/Admin/Order/ChangeStatus",
                data: {
                    id:id,                   
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    var d = response.data;
                    if (d == 0) {
                        btn.text('Đang xử lý');
                    }else if (d == 1) {
                        btn.text('Đã gửi');
                    } else {
                        btn.text('Hủy đơn');
                    }
                },

            });
        });
    }
}
order.init();