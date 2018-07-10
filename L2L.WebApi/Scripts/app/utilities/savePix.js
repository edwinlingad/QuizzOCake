
var pixUtil = (function () {

    /* Utility function to convert a canvas to a BLOB */
    function dataURLToBlob(dataURL) {
        var BASE64_MARKER = ';base64,';
        if (dataURL.indexOf(BASE64_MARKER) == -1) {
            var parts = dataURL.split(',');
            var contentType = parts[0].split(':')[1];
            var raw = parts[1];

            return new Blob([raw], { type: contentType });
        }

        var parts = dataURL.split(BASE64_MARKER);
        var contentType = parts[0].split(':')[1];
        var raw = window.atob(parts[1]);
        var rawLength = raw.length;

        var uInt8Array = new Uint8Array(rawLength);

        for (var i = 0; i < rawLength; ++i) {
            uInt8Array[i] = raw.charCodeAt(i);
        }

        return new Blob([uInt8Array], { type: contentType });
    }
    /* End Utility function to convert a canvas to a BLOB      */

    function saveProfilePix(elemId, canvasElemId, success) {
        var file = document.getElementById(elemId).files[0];
        var reader = new FileReader();

        reader.onloadend = function (e) {
            var data = e.target.result;

            var image = new Image();
            image.onload = function (imageEvent) {
                //var canvas = document.createElement('profile-image-canvas'),
                var canvas = document.getElementById(canvasElemId),
                    max_size = 100,
                    width = image.width,
                    height = image.height;
                if (width > height) {
                    if (width > max_size) {
                        height *= max_size / width;
                        width = max_size;
                    }
                } else {
                    if (height > max_size) {
                        width *= max_size / height;
                        height = max_size;
                    }
                }
                canvas.width = width;
                canvas.height = height;
                canvas.getContext('2d').drawImage(image, 0, 0, width, height);
                var dataUrl = canvas.toDataURL('image/jpeg', 0.8);

                success(dataUrl);
            }

            image.src = data;
        }

        reader.readAsDataURL(file);
    }

    function renderToCanvas(canvas, dataUrl) {
        var ctx = canvas.getContext('2d');
        var img = new Image;
        img.onload = function () {
            ctx.drawImage(img, 0, 0);
        };
        img.src = dataUrl;
    }

    function saveEditorImage(fileNode, canvasNode, size, success) {
        var file = fileNode.files[0];
        var reader = new FileReader();

        reader.onloadend = function (e) {
            var data = e.target.result;

            var image = new Image();
            image.onload = function (imageEvent) {
                var max_size = size,
                    width = image.width,
                    height = image.height;
                if (width > height) {
                    if (width > max_size) {
                        height *= max_size / width;
                        width = max_size;
                    }
                } else {
                    if (height > max_size) {
                        width *= max_size / height;
                        height = max_size;
                    }
                }
                canvasNode.width = width;
                canvasNode.height = height;
                canvasNode.getContext('2d').drawImage(image, 0, 0, width, height);
                var dataUrl = canvasNode.toDataURL('image/jpeg', 0.8);

                success(dataUrl);
            }

            image.src = data;
        }

        reader.readAsDataURL(file);
    }

    return {
        saveProfilePix: saveProfilePix,
        renderToCanvas: renderToCanvas,
        saveEditorImage: saveEditorImage
    }
})();