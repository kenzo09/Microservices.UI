var _products = [];
var selectedProducts = [];
var quantity = 0; $('#quantity').val(0);
navigator.getUserMedia = (navigator.getUserMedia ||
    navigator.webkitGetUserMedia ||
    navigator.mozGetUserMedia ||
    navigator.msGetUserMedia);
var video;
var webcamStream;
var canvas, ctx;

// Simple JavaScript Templating
// John Resig - https://johnresig.com/ - MIT Licensed
(function () {
    var cache = {};

    this.tmpl = function tmpl(str, data) {
        // Figure out if we're getting a template, or if we need to
        // load the template - and be sure to cache the result.
        var fn = !/\W/.test(str) ?
            cache[str] = cache[str] ||
            tmpl(document.getElementById(str).innerHTML) :

            // Generate a reusable function that will serve as a template
            // generator (and which will be cached).
            new Function("obj",
                "var p=[],print=function(){p.push.apply(p,arguments);};" +

                // Introduce the data as local variables using with(){}
                "with(obj){p.push('" +

                // Convert the template into pure JavaScript
                str
                .replace(/[\r\t\n]/g, " ")
                .split("<%").join("\t")
                .replace(/((^|%>)[^\t]*)'/g, "$1\r")
                .replace(/\t=(.*?)%>/g, "',$1,'")
                .split("\t").join("');")
                .split("%>").join("p.push('")
                .split("\r").join("\\'")
                + "');}return p.join('');");

        // Provide some basic currying to the user
        return data ? fn(data) : fn;
    };
})();

$(document).ready(function () {
    $('.steps').hide();
    $('#step1').show();

    $.fn.serializeFormJSON = function () {

        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $('#startWebCam').click(startWebcam);
    $('#takeSnapshot').click(snapshot);
    $('#formStep3').submit(submitFoodRestrictions);

    var connection = new signalR.HubConnectionBuilder().withUrl('/messagehub').build();
    connection.start().catch(err => showErr(err));
    
    connection.on('uicommand', (label, message) => {
        $('.steps').hide();

        if (label.toLowerCase() === 'showwelcomepage') {
            $('#step1').show();
        }
        
        if (label.toLowerCase() === 'showfoodrestrictionsform') {
            $("#step-3").show();            
        }
		//STEP 4
        if (label.toLowerCase() === 'showproductslist') {
            _products = message;
            formatProducts(_products);
            $("#step-4").show();
        }
    });

    //STEP 5
    $('#checkout').click(function () {

        if (selectedProducts.length === 0) {
            alert('Please select a product before checkout');
            return;
        }

        $('.steps').hide();
        $('#step-5').show();

        var orderPayApi = $('#OrderPayApi').val();
        var orderApi = $('#OrderApi').val();

        var d = new Date,
            dformat = [d.getMonth() + 1,
                    d.getDate(),
                    d.getFullYear()].join('/') + ' ' +
                [d.getHours(),
                    d.getMinutes(),
                    d.getSeconds()].join(':');

        var order = {
            Products: selectedProducts,
            RequesterId: $('#RequesterId').val()
        }

        $.ajax({
            type: 'POST',
            url: orderApi,
            data: JSON.stringify(order),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                console.log('OrderId:' + msg);
                $('.prod-summary').html('');
                quantity = 0;

                $('.orderId').html(msg.orderId);
                $('.total').html(msg.total);

                for (var i = 0; i < selectedProducts.length; i++) {
                    $('.prod-summary').append(tmpl("summary_prod_tmpl", selectedProducts[i]));                    
                }
                selectedProducts = [];

                //hard coded (mocking)
                var paymentToPost = {
                    OrderId: msg.orderId,
                    CardNumber: "111222333444555",
                    CardOwnerName: "MIKE JOHNSON",
                    ExpirationDate: "10/01/2017",
                    SecurityCode: "212",
                    StoreId: "8048e9ec-80fe-4bad-bc2a-e4f4a75c834e",
                    RequesterId: $('#RequesterId').val()
                };

                setTimeout(function () {
                    $('.steps').hide();
                    $('#waiting-step6').show();

                    $.ajax({
                        type: 'POST',
                        url: orderPayApi,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: paymentToPost,
                        success: function (msg) {
                        }
                    });
                }, 8000);

            }
        });


    });

    //STEP 6
    connection.on('orderpaid', (label, message) => {
        $('.steps').hide();
        $("#step-6").show();
    });

    //DEBUG
    $('#send-message').click(function () {
        $('#send-message').hide();
        $.ajax({
            type: 'GET',
            url: 'Html/Debug',
            data: "command=" + $('#message').val(),
            success: function (msg) {
                $('#send-message').show();
            }
        }); 
    });

});

//STEP 1
function startWebcam() {

    $('.steps').hide();
    $('#step2').show();
    video = document.querySelector('#video');

    if (navigator.mediaDevices.getUserMedia) {
        navigator.mediaDevices.getUserMedia({ video: true })
            .then(function (stream) {
                video.srcObject = stream;
                webcamStream = stream;
                $('#takeSnapshot').show();
            }).catch(function (error) {
                console.log("Something went wrong!");
            });
    } else {
        console.log("getUserMedia not supported");
    }
}

function stopWebcam() {
    webcamStream.stop();
}

//STEP 2
function init() {
    // Get the canvas and obtain a context for
    // drawing in it
    canvas = document.getElementById('myCanvas');
    ctx = canvas.getContext('2d');
}

function snapshot() {
    // Draws current image from the video element into the canvas
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

    var image = document.getElementById("myCanvas").toDataURL();

    var formData = new FormData();
    formData.append("image", image);

    $('.steps').hide();
    $('#waiting-step2').show();

    $.ajax({
        type: 'POST',
        url: '/Html/Face',
        data: formData,
        processData: false,
        contentType: false,
        success: function (msg) {
            $('#waiting-step3').show();
        }
    });

}

//STEP 3
function submitFoodRestrictions(event) {
    event.preventDefault();
    var formDataJson = $(this).serializeFormJSON();

    formDataJson.Others = formDataJson.Others.split(',');
    formDataJson.RequesterId = $("#RequesterId").val();

    var apiUrl = $('#FoodRestrictionsApiUrl').val();

    $.ajax({
        type: 'POST',
        url: apiUrl,
        data: JSON.stringify(formDataJson),
        success: function (msg) {
            $('.steps').hide();
            $('#waiting-step4').show();
        }
    });
}

function showErr(msg) {
    var listItem = document.createElement('li');
    listItem.setAttribute("style", "color: red");
    listItem.innerText = msg.toString();
    document.getElementById('messages').appendChild(listItem);
}

function formatProducts() {
    $('#products-list').html('');
    for (var i = 0; i < _products.length; i++) {
        _products[i].index = i;

        $('#products-list').append(tmpl("product_tmpl", _products[i]));        
    }

    //register event
    $('.add-cart').click(function () {
        selectedProducts.push(_products[$(this).attr('productIndex')]);
        $(this).parent().html('added to cart!');
        $('#quantity').val(++quantity);
    });
}

