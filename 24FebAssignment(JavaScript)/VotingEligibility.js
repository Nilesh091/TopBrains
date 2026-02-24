let age = 20;
let isCitizen = "Yes";

if (age >= 18) {
    if (isCitizen === "Yes") {
        console.log("Eligible to Vote");
    } else {
        console.log("Not a Citizen");
    }
} else {
    console.log("Underage");
}