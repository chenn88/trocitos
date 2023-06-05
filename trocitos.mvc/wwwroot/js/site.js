$(document).ready(function () {

    var openingTime = 10;
    var closingTime = 21;
    var timeSelect = $('#startTime');

    for (var i = openingTime * 2; i < closingTime * 2; i++) {
        var hour = Math.floor(i / 2);
        var minutes = (i % 2 === 0) ? '00' : '30';


        if (hour < 10) {
            hour = '0' + hour;
        }


        timeSelect.append(new Option(hour + ':' + minutes, hour + ':' + minutes));
    }

    $("#reservationForm").submit(function (event) {
        event.preventDefault();

        var form = $(this);
        var url = form.attr('action');

        $.ajax({
            type: "GET",
            url: "/Reservation/CheckAvailability",
            data: {
                date: form.find('input[name="date"]').val(),
                startTime: form.find('select[name="startTime"]').val(),
                endTime: form.find('input[name="endTime"]').val(),
                capacity: form.find('input[name="capacity"]').val(),
                location: form.find('select[name="location"]').val()
            },
            success: function (data) {
                if (data.success) {
                    $("#availabilityMessage").html("<p style='color: green;'>" + data.message + "</p>");
                    $("#bookingForm").show();
                } else {
                    $("#availabilityMessage").html("<p style='color: red;'>" + data.message + "</p>");
                    $("#bookingForm").hide();
                }
            },

            beforeSend: function () {
                var startTime = form.find('select[name="startTime"]').val();
                console.log("Start time: ", startTime);
            }
        });
    });

    $("#submitReservationForm").submit(function (event) {
        event.preventDefault();

        var form = $(this);

        $.ajax({
            type: "POST",
            url: "/Reservation/Book",
            data: {
                firstName: form.find('input[name="firstName"]').val(),
                surname: form.find('input[name="surname"]').val(),
                phoneNo: form.find('input[name="phoneNo"]').val(),
                email: form.find('input[name="email"]').val(),
                date: $('#date').val(),
                startTime: $('#startTime').val(),
                endTime: $('#endTime').val(),
                capacity: $('#capacity').val(),
                location: $('#location').val()
            },
            success: function (data) {
                if (data.success) {
                    $("#availabilityMessage").html("<p style='color: green;'>" + data.message + "</p>");
                    $("#bookingForm").hide(); // hide booking form
                } else {
                    $("#availabilityMessage").html("<p style='color: red;'>" + data.message + "</p>");
                }
            }
        });
    });
});



(function () {
    'use strict'

    var forms = document.querySelectorAll('.needs-validation')

    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }
                form.classList.add('was-validated')
            }, false)
        })
})()