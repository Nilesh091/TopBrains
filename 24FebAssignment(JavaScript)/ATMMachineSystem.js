const readline = require("readline");

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

let correctPin = 1234;
let balance = 10000;

rl.question("Enter PIN: ", function(pinInput) {

  let enteredPin = parseInt(pinInput);

  if (enteredPin === correctPin) {

    rl.question("Enter amount to withdraw: ", function(amountInput) {

      let withdrawAmount = parseFloat(amountInput);

      if (withdrawAmount >= 0 && balance >= withdrawAmount) {
        balance -= withdrawAmount;
        console.log("Withdrawal successful.");
        console.log("Remaining balance:", balance);
      } else {
        console.log("Insufficient balance.");
      }

      rl.close();
    });

  } else {
    console.log("Incorrect PIN.");
    rl.close();
  }

});