let correctPassword = "admin123";
let attempts = 0;
let enteredPassword;

do {
    enteredPassword = "admin123"; 
    attempts++;

    if (enteredPassword === correctPassword) {
        console.log("Login Successful");
        break;
    } else {
        console.log("Wrong Password");
    }

} while (attempts < 3);

if (attempts === 3 && enteredPassword !== correctPassword) {
    console.log("Account Locked");
}