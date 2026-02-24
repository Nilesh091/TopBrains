let marksList = [95, 85, 72, 60, 88];
for (let marks of marksList) {
    switch (true) {

      case (marks >= 90):
        console.log(marks + " → Grade A");
          break;
      case (marks >= 80):
          console.log(marks + " → Grade B");
            break;
      case (marks >= 70):
          console.log(marks + " → Grade C");
            break;
      default:
            console.log(marks + " → Fail");
    }
}