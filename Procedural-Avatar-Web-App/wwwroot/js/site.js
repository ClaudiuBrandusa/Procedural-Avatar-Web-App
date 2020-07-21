$(document).ready(function () {
    initIndex(); // works only on index page
    // setting to width and height number inputs the sliders' values
    document.getElementById("heightOutputId").value = $("#heightInputId").val();
    document.getElementById("widthOutputId").value = $("#widthInputId").val();
    setImage();
});

function initIndex() { // this function will init all of the Index's page functionalities
    initColorPicker();
}

function initColorPicker() {
    document.getElementById("colorInputId").value = getComputedStyle(document.documentElement).getPropertyValue('--secondary-color').trim();
}

function updateSecondaryColor(color) {
    document.documentElement.style.setProperty('--secondary-color', color);
}

function setImage() {
    // getting the random color checked status
    var randomColorStatus = $('input[name=randomColorStatus]').is(':checked');

    var color = $("#colorInputId").val();
    var width = $("#widthInputId").val();
    var height = $("#heightInputId").val();
    $.ajax({
        type: 'GET',
        url: 'Home/SetImage' + (!randomColorStatus ? '?color=' + '%23' + color.substring(1) + '&' : "?") + 'width=' + width + '&height=' + height,
        success: function (msg) {
            try {
                // here we are initing the SetImage partial view
                $("#main-img").html(msg);
            } catch (err) {
                alert("error at setting the image"); // we are using this for debugging purpose
                return;
            }
        },
        error: function (req, status, error) {
            alert("error:" + error + "\n" + req + "\n" + status); // here we have another alert for debugging purpose
        }
    })
}