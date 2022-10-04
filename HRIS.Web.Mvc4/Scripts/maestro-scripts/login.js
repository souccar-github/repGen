$(document).ready(main);

function main () {
    
    var usernameTextBox = $('#username_textbox');
    var passwordTextBox = $('#password_textbox');
    
    usernameTextBox.keypress(keypress_handler);
    passwordTextBox.keypress(keypress_handler);
    
    usernameTextBox.blur(blur_handler);
    passwordTextBox.blur(blur_handler);
}


function keypress_handler (e) {
    $(this).addClass('inputted-text');
}


function blur_handler () {
    if ($(this).val().length == 0)
        $(this).removeClass ('inputted-text');
}

