javascript:

let title = document.getElementsByClassName("title_tp0qjrp")[0].textContent;

let origin = document.getElementsByClassName("paragraph_p15tm1hb");
let content = "";
for (let para of origin) {
    if (para.className.indexOf("paragraph_") != 0
        || para.className.indexOf("paragraph_") != 0 ) {
        continue;
    }

    content = content + para.textContent;
}

var options = {
    method: 'POST',
    mode: 'cors',
    headers: {
        'Content-Type': 'text/plain; charset=utf-8'
    },
    body: title + "。" + content
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
        console.error('There was a problem with the fetch operation:', error);
    });
