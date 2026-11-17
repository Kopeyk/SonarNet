document.getElementById("order").addEventListener("submit", function (e) {
  e.preventDefault();
  var sum = Number(document.getElementById("sum").value);
  var country = document.getElementById("country").value;
  var shipping = 0;
  if (country === "UA") {
    shipping = sum < 500 ? 70 : 0;
  } else {
    shipping = sum < 1000 ? 250 : 100;
  }
  document.getElementById("result").textContent = "До сплати: " + (sum + shipping) + " грн";
});
