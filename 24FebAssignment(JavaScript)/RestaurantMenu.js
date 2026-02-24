const { ftruncate } = require("fs");
const readline = require("readline");

const rl=readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

rl.question("Enter your choice: ",function(choice){
  switch(choice){
    case "1":
      console.log("Pizza - ₹200");
      break;
    case "2":
      console.log("Burger - ₹150");
      break;
    case "3":
      console.log("Pasta - ₹180");
      break;
    default:
      console.log("Invalid choice.");
  }
  rl.close();
})