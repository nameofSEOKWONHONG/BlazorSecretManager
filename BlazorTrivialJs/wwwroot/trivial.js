function getBrowserInfo() {
    let locale = Intl.DateTimeFormat().resolvedOptions().locale;
    switch (locale) {
        case 'ko':
        case 'ko-KR':
            locale = 'ko-KR';
            break;
        case 'en':
        case 'en-US':
            locale = 'en-US';
            break;
        case 'jp':
        case 'jp-JP':
            locale = 'jp-JP';
            break;                
    }
    return {
        language: locale,
        timezone: Intl.DateTimeFormat().resolvedOptions().timeZone // 사용자의 시간대 (예: "Asia/Seoul")
    }; 
}
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(
        () => console.log("Copied to clipboard"),
        (err) => console.error("Failed to copy text to clipboard", err)
    );
}

function goBack() {
    window.history.back();
}

function goForward() {
    window.history.forward();
}

function scrollToBottom(id) {
    let container = document.getElementById(id);
    if (container) {
        container.scrollTop = container.scrollHeight;
    }
}
