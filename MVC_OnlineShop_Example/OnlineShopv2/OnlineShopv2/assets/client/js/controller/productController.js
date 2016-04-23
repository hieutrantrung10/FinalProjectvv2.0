var product = {
    init:function() {
        product.regEvents();
    },
    regEvents: function() {
        $('.more-images').off('click').on('click', function(e) {
            e.preventDefault();
            $('#image-gallery').modal('show');
        });
    },
    loadGalery: function() {
        $('#show-previous-image, #show-next-image').show();
        $.ajax({
            url: '/Product/LoadImage',
            type: 'POST',
            data: {
                id: ('#image-gallery-').val()
            },
            dataType: 'json',
            
    });
    }
}
product.init();