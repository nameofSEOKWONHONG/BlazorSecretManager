window.getBrowserInfo = () => {
    return {
        language: Intl.DateTimeFormat().resolvedOptions().locale,  // 브라우저 언어 (예: "ko-KR", "en-US")
        timezone: Intl.DateTimeFormat().resolvedOptions().timeZone // 사용자의 시간대 (예: "Asia/Seoul")
    };
};

window.copyToClipboard = (text) => {
    navigator.clipboard.writeText(text).then(
        () => console.log("Copied to clipboard"),
        (err) => console.error("Failed to copy text to clipboard", err)
    );
};

function goBack() {
    window.history.back();
}

function goForward() {
    window.history.forward();
} 