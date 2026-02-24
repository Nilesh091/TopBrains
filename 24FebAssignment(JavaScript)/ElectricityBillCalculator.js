let bill = 0;
const readline = require("readline");

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

rl.question("Enter unit consumed: ", function(unitInput) {
  let units = parseFloat(unitInput);
  if (units <= 100) {
    bill = units * 5;
} else if (units <= 200) {
    bill = units * 7;
} else {
    bill = units * 10;
}

console.log("Total Bill: ₹" + bill);
rl.close();
});