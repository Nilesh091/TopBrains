let account = {
    balance: 10000
};
let choice;
while (true) {

    choice = 3; 
    switch (choice) {
      case 1:
        account.balance += 2000;
        console.log("Deposited. Balance:", account.balance);
        break;
      case 2:
        account.balance -= 1000;
        console.log("Withdrawn. Balance:", account.balance);
        break;

      case 3:
        console.log("Current Balance:", account.balance);
        break;
      case 4:
            console.log("Exiting...");
        break;

      default:
        console.log("Invalid Option");
    }
    if (choice === 4) break;
}

// the code will run in an infinite loop, you can change the value of choice to test different cases.