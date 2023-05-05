javascript:
{
    let title = "";
    let content = "";
    if (document.getElementsByTagName("h1").length == 2) {
        title = document.getElementsByTagName("h1")[1].outerText.replace("\n", "。");

        let origin = document.getElementsByClassName("cmn-section cmn-indent")[0].getElementsByTagName("p");
        for (let para of origin) {
            if (para.attributes.length == 0) {
                content = content + "\r\n" + para.textContent;

            }
        }
    }
    else {
        title = document.getElementsByClassName("title_tp0qjrp")[0].textContent;

        let origin = document.querySelector('[data-track-article-content]');
        for (let para of origin.childNodes) {
            if (para.className.indexOf("paragraph_") == 0) {
                content = content + "\r\n" + para.textContent;
            }
            else if (para.className.indexOf("container_") == 0) {
                if (para.className.indexOf(" ") != -1 || para.className.indexOf(".") != -1) {
                    content = content + "\r\n" + para.textContent + "。";
                }
            }
        }
    }


    let options = {
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
            alert("Error:" + error);
        });
}