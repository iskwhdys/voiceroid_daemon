javascript:

navigator.clipboard.readText().then(function (text) {
    console.log(text);

    var options = {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Content-Type': 'text/plain; charset=utf-8'
        },
        body: text
    };

    fetch('http://localhost:8080/play', options)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.body;
        })
        .then(data => {
            console.log(data);
        })
        .catch(error => {
            alert("Error:" + error);
        });
});
