let student = {
    name: "Nilu",
    math: 80,
    science: 75,
    english: 90
};
for (let key in student) {
    console.log(key + ":", student[key]);
}