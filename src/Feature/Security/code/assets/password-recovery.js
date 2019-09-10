function checkPasswordMatch() {
    var password = $("#NewPassword").val();
    var confirmPassword = $("#ConfirmPassword").val();
    if (password != confirmPassword) {
        $("#divCheckPasswordMatch").html("Sorry, the passwords do not match.");
        $(".submit-row input").attr("disabled", "disabled");
    } else {
        $("#divCheckPasswordMatch").html("");
        $(".submit-row input").removeAttr("disabled");
    }
}
$(document).ready(function () {
    $("#NewPassword, #ConfirmPassword").keyup(checkPasswordMatch);

    $(".validation-summary-errors").insertAfter($(".form-row:last"));
});