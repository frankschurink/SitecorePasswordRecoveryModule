function replaceMessageBarText() {
  $(".sc-messageBar-messageText").text("We have send you an email with a password reset link. If you don't receive the email within 5 minutes, please check your junk/spam folder. Otherwise, please contact your administrator.");
}

$(document).ready(function() {
  replaceMessageBarText();
});
