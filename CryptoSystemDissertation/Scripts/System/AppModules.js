var ChaosParams = (function () {
    this.lambda = "";
    this.x0 = "";
    this.c0 = "";
    this.t = "";
    this.a = "";
    this.b = "";

    this.SetParams = function (lambda, x0, c0, t, a, b) {
        ChaosParams.lambda = lambda;
        ChaosParams.x0 = x0;
        ChaosParams.c0 = c0;
        ChaosParams.t = t;
        ChaosParams.a = a;
        ChaosParams.b = b;
    }

    this.GetLambda = function () {
        return ChaosParams.lambda;
    }

    this.GetX0 = function () {
        return ChaosParams.x0;
    }

    this.GetC0 = function () {
        return ChaosParams.c0;
    }

    this.GetT = function () {
        return ChaosParams.t;
    }

    this.GetA = function () {
        return ChaosParams.a;
    }

    this.GetB = function () {
        return ChaosParams.b;
    }

    return {
        SetParams: SetParams,
        GetLambda: GetLambda,
        GetX0: GetX0,
        GetC0: GetC0,
        GetT: GetT,
        GetA: GetA,
        GetB:GetB
    }

}());

var ChaoticImage = (function () {

    this.chaoticImage = "";
    this.chaoticImageId = "";

    this.SetImageId = function (id) {
        ChaoticImage.chaoticImageId = id;
    }

    this.GetImageId = function () {
        return ChaoticImage.chaoticImageId;
    }

    this.SetImage = function (imageData) {
        ChaoticImage.chaoticImage = imageData;
    }

    this.GetImage = function () {
        return ChaoticImage.chaoticImage;
    }

    return {
        SetImage: SetImage,
        GetImage: GetImage,
        SetImageId: SetImageId,
        GetImageId: GetImageId
    }
}());

var MyOperations = (function () {

    this.GetPicturesForMe = function () {
        getPicturesForMe().then(function (result) {
            var myPics = result;

            var $myPicturesHolder = $("#myPicturesHolder");
            $myPicturesHolder.empty();
            if (myPics.ImageId.length == 0) {
                var $noResults = $("<div class='no-results'>There are no images for you.</div>");
                $myPicturesHolder.append($noResults);
                return;
            }

            for(var i = 0; i < myPics.ImageId.length; i++)
            {
                var crtImage = {
                    ImageId: myPics.ImageId[i],
                    SenderId: myPics.SenderId[i],
                    SenderName: myPics.SenderName[i]
                }

                $newImageHolder = $('<div class="row"> <div class="col-lg-12"> <div class="media"> <a class="pull-left" href="#"> <img class="media-object dp img-circle" src="http://icons.iconarchive.com/icons/martz90/circle/128/messages-icon.png" style="width: 50px;height:50px;"> </a> <div class="media-body"> <h4 class="media-heading">' + crtImage.SenderName + '</h4><small> sent you an image. </small><hr style="margin:8px auto"> <button class="btn btn-primary full-width" onclick="MyOperations.RedirectToMyPicture(\''+crtImage.ImageId+'\',\''+crtImage.SenderId+'\')"> See image <span class="glyphicon glyphicon-picture"></span></button></div> </div> </div> </div>');
                $myPicturesHolder.append($newImageHolder);
            }
        })
    };

    this.GetPicture = function (imageId, senderId) {
        
        //rsa keys
        var rsa = RSACrypto.GetNewRsaProvider();
        key = RSACrypto.GetRSAKeyPair();
        rsa.FromXmlString(key);
        var publicKey = rsa.ToXmlString(false);
        var data = {
            ImageId: imageId,
            SenderId: senderId,
            RSAKey: publicKey
        }

        getPicture(data).then(function (data) {
            if (!data.Parmateres)
                return;
            var encrypted = data.Image.split('$')[0];
            $('.preview-initial').attr('src', encrypted).width("100%");
            var decryptionParams = RSACrypto.Decrypt(data.Parmateres);
            var $decryptionParamsXml = $($.parseXML(decryptionParams));
            var $params = $decryptionParamsXml.find('Parameters');
            var lambda = $params.attr("Lambda");
            var x0 = $params.attr("X");
            var c0 = $params.attr("C0");
            var t = $params.attr("T");
            var a = $params.attr("A");
            var b = $params.attr("B");
            ChaosParams.SetParams(lambda, x0, c0, t, a, b);
            var startdecrypt = +new Date();
            Crypto.Decrypt();
            var enddecrypt = +new Date();
            console.log("Decrypt" + (enddecrypt - startdecrypt));
            $('.preview-encrypted').attr('src', data.Image.split('$')[1]).width("100%");
        })

    }
    
    this.RedirectToMyPicture = function (imageId, senderId) {
        var url = "/ImageBoard/GetImageDetails?ImageId=" + imageId + "&SenderId=" + senderId + "";
        window.location.href = url;
    }

    var getPicture = function (data) {
        
        var deferred = $.Deferred();

        $.ajax({
            type: "POST",
            url: this.serverGetEncryptImageEndpoint,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                deferred.resolve(data);
            },
            error: function () {
                console.log("Error getting encrypted params...")
            }
        });
        return deferred.promise();
    }

    var getPicturesForMe = function () {

        this.serverGetPicturesForMeEndpoint = "/ImageBoard/ReceiveImage/";
        var deferred = $.Deferred();

        $.ajax({
            type: "GET",
            url: this.serverGetPicturesForMeEndpoint,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                deferred.resolve(data);
            },
            error: function (message) {
                console.log("Error getting encrypted params...")
            }
        });
        return deferred.promise();
    };

    return {
        GetPicturesForMe: GetPicturesForMe,
        GetPicture:GetPicture,
        RedirectToMyPicture: RedirectToMyPicture
    }

}());

var RSACrypto = (function () {

    this.key = "";
    this.keySize = 2048;
    this.serverGetEncryptParamsEndpoint = "/ImageBoard/ParametersImage/";
    this.serverSendEncryptedImageEndpoint = "/ImageBoard/ImageString/"
    this.serverGetEncryptImageEndpoint = "/ImageBoard/ReceiveImage";

    this.GetRSAKeyPair = function () {
        var encryptActiveX = new ActiveXObject("csharpAx.Crypto");
        if (encryptActiveX != null) {
            return encryptActiveX.GetRSAKeyPair(keySize);
        }
        else {
            alert("Library not loaded. Please retrieve the dll file.");
            return null;
        }
    }
    this.GetNewRsaProvider = function () {
        return new System.Security.Cryptography.RSACryptoServiceProvider();
    }

    this.SendPublicKey = function (receiverID) {
        //rsa keys
        var rsa = GetNewRsaProvider();
        var startrsa = +new Date();
        key = GetRSAKeyPair();
        var endrsa = +new Date();
        console.log("RSA time: " + (endrsa - startrsa));
        rsa.FromXmlString(key);
        var publicKey = rsa.ToXmlString(false);
        var data = {
                ReceiverId: receiverID,
                Image: "",
                RSAKey: publicKey
        }

        getEncryptedParams(data).then(function (data) {
            if (!data.encryptParam && !data.ImageId)
                return;

            ChaoticImage.SetImageId(data.ImageId);
            var encryptionParams = Decrypt(data.encryptParam);
            var $encryptionParamsXml = $($.parseXML(encryptionParams));
            var $params = $encryptionParamsXml.find('Parameters');
            var lambda = $params.attr("Lambda");
            var x0 = $params.attr("X");
            var c0 = $params.attr("C0");
            var t = $params.attr("T");
            var a = $params.attr("A");
            var b = $params.attr("B");
            ChaosParams.SetParams(lambda, x0, c0, t, a, b);
            var startencrypt = +new Date();
            Crypto.Encrypt();
            var endencrypt = +new Date();
            console.log("Encryption time:" + (endencrypt - startencrypt));
            $("#btnSendToReceiver").removeClass("hidden");
        })
    }

    this.SendEncryptedImage = function (receiverID) {
        var data = {
            ReceiverId: receiverID,
            Image: ChaoticImage.GetImage(),
            ImageId: ChaoticImage.GetImageId(),
            RSAKey: ""
        }

        sendEncryptedImage(data).then(function (data) { });

        setTimeout(function () {          
            $('#btnSendToReceiver').remove();
        }, 500);

 
    }

    this.Decrypt = function (encryptedBytes) {
        var xmlParams = key;

        //rsa keys
        var rsa = GetNewRsaProvider();
        rsa.FromXmlString(xmlParams);
        var decryptedBytes = rsa.Decrypt(System.Convert.FromBase64String(encryptedBytes), false);
        // ------------------------------------------------
        // Display the decrypted data.
        var decryptedString = System.Text.Encoding.UTF8.GetString(decryptedBytes);
        return decryptedString;
    }

    var getEncryptedParams = function (data) {

       var deferred = $.Deferred();

        $.ajax({
            type: "POST",
            url: this.serverGetEncryptParamsEndpoint,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                deferred.resolve(data);
            },
            error: function () {
                console.log("Error getting encrypted params...")
            }
        });
        return deferred.promise();
    }

    var sendEncryptedImage = function (data) {

        var deferred = $.Deferred();

        $.ajax({
            type: "POST",
            url: this.serverSendEncryptedImageEndpoint,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                deferred.resolve(data);
            },
            error: function (msg) {
                console.log("Error sending encrypted image...")
            }
        });
        return deferred.promise();
    }

    return {
        GetNewRsaProvider: this.GetNewRsaProvider,
        GetRSAKeyPair:this.GetRSAKeyPair,
        SendPublicKey: this.SendPublicKey,
        SendEncryptedImage: this.SendEncryptedImage,
        Decrypt: this.Decrypt
    }

}());

var Crypto = (function () {

    var Encrypt = function (e) {

        var encryptActiveX = new ActiveXObject("csharpAx.Crypto");
        if (encryptActiveX != null) {
            encryptActiveX.SetEncryptionParameters(ChaosParams.GetX0(), ChaosParams.GetLambda(), ChaosParams.GetC0(), ChaosParams.GetT(), ChaosParams.GetA(), ChaosParams.GetB());
            var header = $('.preview-initial').attr('src').split(",")[0];
            var confussedImageSrc = header + "," + encryptActiveX.EncryptImage($('.preview-initial').attr('src').split(",")[1]);
            ChaoticImage.SetImage(confussedImageSrc + "$" + $('.preview-initial').attr('src'));
            $('.preview-encrypted').attr('src', confussedImageSrc).width("100%");
        }
        else
            alert("Library not loaded. Please retrieve the dll file.");
    }

    var Decrypt = function (e) {
        var decryptActiveX = new ActiveXObject("csharpAx.Crypto");
        if (decryptActiveX != null) {
            decryptActiveX.SetEncryptionParameters(ChaosParams.GetX0(), ChaosParams.GetLambda(), ChaosParams.GetC0(), ChaosParams.GetT(), ChaosParams.GetA(), ChaosParams.GetB());
            var header = $('.preview-initial').attr('src').split(",")[0];
            var decryptedImageSrc = header + "," + decryptActiveX.DecryptImage($('.preview-initial').attr('src').split(",")[1]);
            //$('.preview-encrypted').attr('src', decryptedImageSrc).width("100%");
        }
        else
            alert("Library not loaded. Please retrieve the dll file.");
    }

    return {
        Encrypt: Encrypt,
        Decrypt: Decrypt
    }

}());

var ImageProcessor = (function () {

    var showImage = function (input, receiverId) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('.preview-initial')
                  .attr('src', e.target.result)
                  .width("100%")
                RSACrypto.SendPublicKey(receiverId);
            };
            reader.readAsDataURL(input.files[0]);
            
        }
    }

    return {
        showImage: showImage,
    }
}());
