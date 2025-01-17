﻿var indice = indice || {};

(function () {
    indice.mfaViewModelFactory = function (viewModelParams) {
        var self;
        var host = window.location.protocol + '//' + window.location.host;
        return {
            self: undefined,
            browserId: undefined,
            init: function () {
                self = this;
                if (viewModelParams.deliveryChannel === 'PushNotification') {
                    self.startSignalRConnection();
                }
                viewModelParams.$rememberClientCheckbox.change(function () {
                    if (!self.browserId && this.checked) {
                        const fpPromise = FingerprintJS.load();
                        fpPromise.then(fp => fp.get()).then(result => {
                            self.browserId = result.visitorId;
                            viewModelParams.$deviceIdInput.val(self.browserId);
                        });
                    }
                });
            },
            onConnected: function (connection) {
                var requestToken = $("[name='__RequestVerificationToken']").val();
                $.ajax({
                    url: host + '/login/mfa/notify',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        connectionId: connection.connectionId
                    }),
                    headers: {
                        'X-XSRF-TOKEN': requestToken
                    },
                    dataType: 'json',
                    success: function (data, textStatus, jqXHR) { },
                    error: function (jqXHR, textStatus, errorThrown) { }
                });
            },
            startSignalRConnection: function () {
                const connection = new signalR
                    .HubConnectionBuilder()
                    .withUrl('/mfa')
                    .build();
                connection.on('LoginApproved', function (otpCode) {
                    viewModelParams.$otpCodeInput.val(otpCode);
                    viewModelParams.$mfaForm.submit();
                });
                connection.on('LoginRejected', function () { });
                connection.start()
                    .then(() => self.onConnected(connection))
                    .catch(error => console.error(error.message));
            }
        }
    }
})();
