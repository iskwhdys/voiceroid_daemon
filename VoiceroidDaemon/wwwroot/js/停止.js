javascript:
{
    var options = {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Content-Type': 'text/plain; charset=utf-8'
        }
    };

    fetch('http://localhost:8080/stop', options)
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
}