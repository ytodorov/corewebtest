// Write your JavaScript code.
setCorrectTime();
setInterval(setCorrectTime, 1000);
debugger;
function setCorrectTime()
{
    var span = $("#spanTime");
    var time = new Date();
    span.text(time.toLocaleString());
}

