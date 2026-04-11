// function myFunction() {
//     // console.log("Hello, World!");
//     if(true){
//       let a = 10;
//       console.log('inside the block: ' + a);
//     }
//     let a = 20;
//     console.log('outside the block: ' + a);
// }

// myFunction();

// function MyFunction2(){
//     var a = 10;
//     console.log('inside the function: ' + a);
//     if(true){
//         var a = 20;
//         console.log('inside the block: ' + a);
//     }
//     console.log('outside the block: ' + a);
// }

// MyFunction2();



// function infiniteLoop():never {
//         // while (true) {
//         //     console.log("This is an infinite loop.");
//         // }
//     // console.log("This function will throw an error instead of looping infinitely.");
//     throw new Error("This function will throw an error instead of looping infinitely.");
// }
// infiniteLoop();

let myvar= 10;
console.log(myvar);
console.log(typeof myvar);


function sum(a: number, b: number):number {
    return a + b;
}

let result:number = sum(5, 10);
console.log("The sum is: " + result);

function concat(value1 :string, value2: string): string {
    return value1 + value2;
}

let result2:string = concat("Hello, ", "World!");
console.log(result2);

function printMessage(message: string|number): void {
    console.log("Message: " + message);
}
printMessage("This is a string message.");
printMessage(12345);