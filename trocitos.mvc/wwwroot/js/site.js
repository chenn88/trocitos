$(document).ready(function () {

    var openingTime = 10;
    var closingTime = 21;
    var timeSelect = $('#start-time');

    for (var i = openingTime * 2; i < closingTime * 2; i++) {
        var hour = Math.floor(i / 2);
        var minutes = (i % 2 === 0) ? '00' : '30';


        if (hour < 10) {
            hour = '0' + hour;
        }


        timeSelect.append(new Option(hour + ':' + minutes, hour + ':' + minutes));
    }

    $("#reservation-form").submit(function (event) {
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
                    $("#availability-message").html("<p style='color: green;'>" + data.message + "</p>");
                    $("#booking-form").show();
                } else {
                    $("#availability-message").html("<p style='color: red;'>" + data.message + "</p>");
                    $("#booking-form").hide();
                }
            },

            beforeSend: function () {
                var startTime = form.find('select[name="startTime"]').val();
                console.log("Start time: ", startTime);
            }
        });
    });

    $("#submit-reservation-form").submit(function (event) {
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
                startTime: $('#start-time').val(),
                endTime: $('#end-time').val(),
                capacity: $('#capacity').val(),
                location: $('#location').val()
            },
            success: function (data) {
                if (data.success) {
                    $("#confirmation-message").html(`<p>${data.message} <br> Reservation number: ${data.reservationId} <br> Booking date: ${data.bookingDate} <br> Start time: ${data.startTime} <br> Number of people: ${data.partySize}</p>`);
                    $("#availability-message").hide();
                    $("#booking-form").hide();
                    $("#availability-check-form").hide();

                } else {
                    $("#availability-message").html("<p style='color: red;'>" + data.message + "</p>");
                }
            }
        });
    });

    $("#check-btn").click(function () {
        var reservationId = $("#reservation-id").val();
        var contactInfo = $("#contact-info").val();

        $.get(`/Reservation/ReservationExists?reservationId=${reservationId}&contactInfo=${contactInfo}`, function (data) {
            if (data.exists) {
                $("#message").text("Reservation found. Please click 'Cancel Reservation' to cancel.");
                $("#cancel-btn").show();
            } else {
                $("#message").text("Reservation not found");
                $("#cancel-btn").hide();
            }
        });
    });

    $("#cancel-btn").click(function () {
        var reservationId = $("#reservation-id").val();
        var contactInfo = $("#contact-info").val();

        cancelReservation(reservationId, contactInfo);
    });

    function cancelReservation(reservationId, contactInfo) {
        $.ajax({
            type: "PUT",
            url: "/Reservation/CancelReservation",
            data: {
                reservationId: reservationId,
                contactInfo: contactInfo
            },
            success: function (data) {
                console.log("Cancellation success:", data);
                $("#message").text(data.message);
            },
            error: function (error) {
                console.error('Cancellation error:', error);
            },
        });
    }

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