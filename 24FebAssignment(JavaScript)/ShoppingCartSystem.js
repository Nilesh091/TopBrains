let cart = [
    { item: "Laptop",price: 50000 },
    { item: "Mouse", price: 500 },
    { item: "Keyboard",price: 1500 }
];
let totalPrice = 0;

for (let product of cart) {
    console.log(product.item + " - ₹" + product.price);
    totalPrice += product.price;
}

let gst = totalPrice * 0.18;
let finalAmount = totalPrice + gst;
console.log("Total:", totalPrice);
console.log("GST (18%):", gst);
console.log("Final Amount:", finalAmount);