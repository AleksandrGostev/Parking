Parking = {
    Url: {
        GetInfo: "",
        LeavePlace: "",
        HomeLink: "",
        Stats: ""
    }
};

$(function () {

    renderTime();
    setInterval(function () {
        renderTime();
    }, 1000);

    $('.place-area').click(function () {
        var place = $(this).parents('.parking-place');
        var row = place.attr('data-place-row');
        var number = place.attr('data-number');
        var isReserved = place.attr('data-client-place');
        $.ajax({
            url: Parking.Url.GetInfo,
            type: 'POST',
            data: {
                number: number,
                row: row,
                reserved: isReserved
            },
            success: function (html) {
                $('#main-modal').html(html);
                $('.modal').modal('show');
            }
        });
    });

    $('body').on('click', '.take-place', function () {
        var $form = $(this).parents('.modal').find('form');
        $form.submit(function () {
            var isClient = $('.client').is(':checked');
            if (!isClient && $('.reserved').val() != "") {
                $('.modal-body').find('.err-message').show();
                return false;
            }
            var action = $(this).attr('action');
            var data = {
                row: $form.find('.row').val(),
                number: $form.find('.number').val(),
                isClient: isClient
            }
            $.ajax({
                url: action,
                data: data,
                type: 'POST',
                success: function (result) {
                    if (result) {
                        window.location.href = Parking.Url.HomeLink;
                    }
                }
            });
            return false;
        });
        $form.submit();
    });

    $('body').on('click', '.leave-place', function () {
        var $modal = $(this).parents('.modal');
        var row = $modal.find('.row').val();
        var number = $modal.find('.number').val();

        $.ajax({
            url: Parking.Url.LeavePlace,
            type: 'POST',
            data: {
                row: row,
                number: number
            },
            success: function (result) {
                if (result) {
                    window.location.href = Parking.Url.HomeLink;
                }
            }
        });
    });

    $('.btn-stats').click(function () {
        $.ajax({
            url: Parking.Url.Stats,
            type: 'POST',
            success: function (html) {
                $('#main-modal').html(html);
                $('.modal').modal('show');
            }
        });
    });
});

function renderTime() {
    var currentDate = new Date();
    var newDateTime = currentDate.getDate() + "/"
                + (currentDate.getMonth() + 1) + "/"
                + currentDate.getFullYear() + " "
                + currentDate.getHours() + ":"
                + currentDate.getMinutes() + ":"
                + currentDate.getSeconds();
    $('.time').find('h2').text(newDateTime);
}