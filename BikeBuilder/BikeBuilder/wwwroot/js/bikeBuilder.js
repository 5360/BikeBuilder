


var container = document.querySelector("#unity-container");
var canvas = document.querySelector("#unity-canvas");
var loadingBar = document.querySelector("#unity-loading-bar");
var progressBarFull = document.querySelector("#unity-progress-bar-full");
var fullscreenButton = document.querySelector("#unity-fullscreen-button");
var warningBanner = document.querySelector("#unity-warning");

// Shows a temporary message banner/ribbon for a few seconds, or
// a permanent error message on top of the canvas if type=='error'.
// If type=='warning', a yellow highlight color is used.
// Modify or remove this function to customize the visually presented
// way that non-critical warnings and error messages are presented to the
// user.
function unityShowBanner(msg, type) {
    function updateBannerVisibility() {
        warningBanner.style.display = warningBanner.children.length ? 'block' : 'none';
    }
    var div = document.createElement('div');
    div.innerHTML = msg;
    warningBanner.appendChild(div);
    if (type == 'error') div.style = 'background: red; padding: 10px;';
    else {
        if (type == 'warning') div.style = 'background: yellow; padding: 10px;';
        setTimeout(function () {
            warningBanner.removeChild(div);
            updateBannerVisibility();
        }, 5000);
    }
    updateBannerVisibility();
}

var buildUrl = "Build";
var loaderUrl = buildUrl + "/Build.loader.js";
var config = {
    dataUrl: buildUrl + "/Build.data.unityweb",
    frameworkUrl: buildUrl + "/Build.framework.js.unityweb",
    codeUrl: buildUrl + "/Build.wasm.unityweb",
    streamingAssetsUrl: "StreamingAssets",
    companyName: "BikeBuilder",
    productName: "BikeBuilder",
    productVersion: "0.1",
    showBanner: unityShowBanner,
};

// By default Unity keeps WebGL canvas render target size matched with
// the DOM size of the canvas element (scaled by window.devicePixelRatio)
// Set this to false if you want to decouple this synchronization from
// happening inside the engine, and you would instead like to size up
// the canvas DOM size and WebGL render target sizes yourself.
// config.matchWebGLToCanvasSize = false;

if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
    // Mobile device style: fill the whole browser client area with the game canvas:

    var meta = document.createElement('meta');
    meta.name = 'viewport';
    meta.content = 'width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes';
    document.getElementsByTagName('head')[0].appendChild(meta);
    container.className = "unity-mobile";

    // To lower canvas resolution on mobile devices to gain some
    // performance, uncomment the following line:
    // config.devicePixelRatio = 1;

    canvas.style.width = window.innerWidth + 'px';
    canvas.style.height = window.innerHeight + 'px';

    unityShowBanner('WebGL builds are not supported on mobile devices.');
} else {
    // Desktop style: Render the game canvas in a window that can be maximized to fullscreen:

    canvas.style.width = "1340px";
    canvas.style.height = "870px";
}

loadingBar.style.display = "block";

var bikeBuilderInstance = null;

var script = document.createElement("script");
script.src = loaderUrl;
script.onload = () => {
    createUnityInstance(canvas, config, (progress) => {
        progressBarFull.style.width = 100 * progress + "%";
    }).then((unityInstance) => {
        loadingBar.style.display = "none";
        fullscreenButton.onclick = () => {
            unityInstance.SetFullscreen(1);
        };
        bikeBuilderInstance = unityInstance;
    }).catch((message) => {
        alert(message);
    });
};
document.body.appendChild(script);


var frameName

function viewFrame(name, color) {
    //debugger;
    bikeBuilderInstance.SendMessage("Bike", "ViewFrame", name + "/" + color);
}

function viewWheels(name) {
    wheelsName = name;    
    bikeBuilderInstance.SendMessage("Bike", "ViewWheels", name);
}

$('.card-list').on('click', '.part-card-container', function (e) {
    console.log(e);
    /*debugger;*/
    if (!e.target.classList.contains('form-select') || e.target.disabled) {
        $('.card-list .selected-part').removeClass('selected-part');
        $('.part-card-container > .form-select').prop('disabled', true);
        $('.form-select', this).prop('disabled', false);
        $(this).addClass('selected-part');
    }
});

$('.frame-card').on('click', function (e) {
    //console.log(e);
    //debugger;

    if (e.target.classList.contains('form-select') && !e.target.disabled) {
        return;
    }

    var frame = $(this).attr("id");

    var color = $('.form-select', this).find(":selected").text();

    viewFrame(frame, color)
});

$('.wheels-card').on('click', function (e) {
    //console.log(e);
    //debugger;

    if (e.target.classList.contains('form-select') && !e.target.disabled) {
        return;
    }

    var wheels = $(this).attr("id");

    var color = $('.form-select', this).find(":selected").text();

    viewWheels(wheels, color);
});

$('.frame-card > .color-select').change(function () {
    //debugger;
    var frame = $(this).parents('.frame-card').attr("id");
    viewFrame(frame, $(this).val() )
});

$('.wheels-card > .color-select').change(function () {
    //debugger;
    var wheels = $(this).parents('.wheels-card').attr("id");
    viewWheels(wheels, $(this).val())
});
