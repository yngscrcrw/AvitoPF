const proxyAddresses = [
  { host: "45.14.221.181", port: 5462 },
  { host: "194.61.234.192", port: 5462 },
  { host: "193.3.18.232", port: 5462 },
  { host: "188.95.71.31", port: 5462 },
  { host: "94.158.188.5", port: 5462 }
];

const username = "user184918";
const password = "wfvcs3";

chrome.runtime.onInstalled.addListener(() => {
  const randomIndex = Math.floor(Math.random() * proxyAddresses.length);
  const selectedProxy = proxyAddresses[randomIndex];

  const config = {
    mode: "fixed_servers",
    rules: {
      singleProxy: {
        scheme: "http",
        host: selectedProxy.host,
        port: selectedProxy.port
      },
      bypassList: ["localhost"]
    }
  };

  chrome.proxy.settings.set({ value: config, scope: "regular" }, () => {});

  chrome.webRequest.onAuthRequired.addListener(
    (details) => {
      return {
        authCredentials: {
          username: username,
          password: password
        }
      };
    },
    { urls: ["<all_urls>"] },
    ["blocking"]
  );
});