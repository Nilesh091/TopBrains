
function fetchInputValues() {
  const firstValue = parseFloat(document.getElementById("firstNumber").value);
  const secondValue = parseFloat(document.getElementById("secondNumber").value);

  if (isNaN(firstValue) || isNaN(secondValue)) {
    alert("Please enter valid numbers");
    return null;
  }

  return { firstValue, secondValue };
}


function displayResult(expression) {
  document.getElementById("output").innerText = expression;
}


function handleAddition() {
  const values = fetchInputValues();
  if (!values) return;

  const sum = values.firstValue + values.secondValue;
  displayResult(`${values.firstValue} + ${values.secondValue} = ${sum}`);
}


function handleSubtraction() {
  const values = fetchInputValues();
  if (!values) return;

  const difference = values.firstValue - values.secondValue;
  displayResult(`${values.firstValue} - ${values.secondValue} = ${difference}`);
}


function handleMultiplication() {
  const values = fetchInputValues();
  if (!values) return;

  const product = values.firstValue * values.secondValue;
  displayResult(`${values.firstValue} × ${values.secondValue} = ${product}`);
}


function handleDivision() {
  const values = fetchInputValues();
  if (!values) return;

  if (values.secondValue === 0) {
    alert("Cannot divide by zero");
    return;
  }

  const quotient = (values.firstValue / values.secondValue).toFixed(2);
  displayResult(`${values.firstValue} ÷ ${values.secondValue} = ${quotient}`);
}