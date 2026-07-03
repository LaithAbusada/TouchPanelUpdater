# Network / API Call Inventory — Innovo TP4 Updater

Audit of every outbound network call in the codebase (updated 2026-07-03, after the PIN-login change).

## 1. Calls to innovo.net (active)

| # | Endpoint | File | Line(s) | Purpose | Client |
|---|----------|------|---------|---------|--------|
| 1 | `https://innovo.net/repo/TP4/app.json` | `PasswordForm.cs` | 86 | Self-update check on startup — compares saved version against latest, provides download `filepath` link | RestSharp (`RestClient`) |
| 2 | `https://innovo.net/repo/TP4/update_apps.json` | `UpdateAppForm.cs` | 45, 258 | Catalog of installable panel apps (version, filename, packageName, image, type) | `HttpClient.GetStringAsync` |
| 3 | `https://innovo.net/repo/TP4/reset_apps.json` | `ResetForm.cs` | 44 | Catalog of apps shown on the Reset screen | `HttpClient.GetStringAsync` |
| 4 | `https://innovo.net/repo/TP4/reset_apps.json` | `FactoryDefaultForm.cs` | 299, 519 | Catalog of apps for factory-default reinstall | `HttpClient.GetStringAsync` |
| 5 | `https://innovo.net/repo/TP4/Apks/{filename}` | `UpdateAppForm.cs` | 322 | APK / zip download for app updates | `WebClient.DownloadFileTaskAsync` |
| 6 | `https://innovo.net/repo/TP4/Apks/{filename}` | `FactoryDefaultForm.cs` | 359 | APK / zip download during factory default | `WebClient.DownloadFileTaskAsync` |
| 7 | `https://innovo.net/repo/TP4/images/{imageFileName}` | `UpdateAppForm.cs` | 429 | App tile icon download | `HttpClient.GetByteArrayAsync` |
| 8 | `https://innovo.net/repo/TP4/images/{imageFileName}` | `ResetForm.cs` | 143 | App tile icon download | `HttpClient.GetByteArrayAsync` |
| 9 | `https://innovo.net/repo/TP4/images/{imageFileName}` | `FactoryDefaultForm.cs` | 39 | App tile icon download | `HttpClient.GetByteArrayAsync` |

## 2. Other API calls (non-innovo.net, active)

| # | Endpoint | File | Line(s) | Purpose | Client |
|---|----------|------|---------|---------|--------|
| 10 | `http://worldtimeapi.org/api/timezone/Etc/UTC` | `PasswordForm.cs` (URL defined in `App.config` line 30, key `ServerTimeApiUrl`) | 144 | Fetches server UTC time for the license-expiry check (anti clock-tampering). **Note: plain HTTP, not HTTPS.** | RestSharp (`RestClient`) |

## 3. Browser links (opened in default browser, not API calls)

| # | URL | File | Line(s) | Trigger |
|---|-----|------|---------|---------|
| 11 | `https://wiki.innovo.net/en/4inadapter` | `ConnectDisconnectForm.cs` | 177 | "4\" Touch Panel Wiki" link |
| 12 | `https://wiki.innovo.net/en/5intp` | `ConnectDisconnectForm.cs` | 193 | "5\" Touch Panel Wiki" link |
| 13 | `https://en.wikipedia.org/wiki/List_of_tz_database_time_zones` | `TimeZoneForm.cs` | 132 | Timezone info help link |
| 14 | Update download link (`filepath` value from `app.json`) | `PasswordForm.cs` | 56 | Opened via `Process.Start(link)` when an update is available |

## 4. Removed on 2026-07-03 (PIN-login / projects removal)

These endpoints are no longer called anywhere in the code:

| Endpoint | Was in | Purpose (former) |
|----------|--------|------------------|
| `https://innovo.net/wp-json/api/v1/token` | `PasswordForm.cs` | Dealer username/password login (JWT) — replaced by local PIN check |
| `https://showroom.innovo.net/LoginDealer.php` | `PasswordForm.cs` | Dealer-ID lookup after login |
| `https://innovo.net/my-account` | `PasswordForm.cs` | "Need Help with Login?" link |
| `https://auth.innovo.net/GetProjects.php` | `ConnectDisconnectForm.cs` | Fetch dealer's projects |
| `https://auth.innovo.net/GetPanels.php` | `ConnectDisconnectForm.cs` | Fetch panels for a project |
| `https://auth.innovo.net/AddProject.php` | `ConnectDisconnectForm.cs` | Create project |
| `https://auth.innovo.net/AddPanel.php` | `ConnectDisconnectForm.cs` | Create panel |
| `https://auth.innovo.net/EditProject.php` | `ConnectDisconnectForm.cs` | Rename project |
| `https://auth.innovo.net/EditPanel.php` | `ConnectDisconnectForm.cs` | Edit panel name/IP/port |
| `https://auth.innovo.net/DeleteProject.php` | `ConnectDisconnectForm.cs` | Delete project |
| `https://auth.innovo.net/DeletePanel.php` | `ConnectDisconnectForm.cs` | Delete panel |

## Notes / flags

- **`worldtimeapi.org` is called over plain HTTP** (`App.config` line 30). Anyone on the network path can spoof the response and bypass or trigger the expiry check. Switch to `https://worldtimeapi.org/...` (the API supports HTTPS) or an innovo.net-hosted time endpoint.
- **Single point of dependency:** all app catalogs, APKs, icons, and the self-update check live under `https://innovo.net/repo/TP4/`. If that path moves, `PasswordForm.cs`, `UpdateAppForm.cs`, `ResetForm.cs`, and `FactoryDefaultForm.cs` all need updating — the URL is hard-coded in each file (9 call sites) rather than centralized in `App.config`.
- **No authentication on any remaining call:** after the login removal, every remaining endpoint is anonymous GET. APK downloads have no signature/hash verification before `adb install`.
- Local ADB traffic (`adb connect <panel-ip>:5555` etc.) goes to the panel on the LAN, not the internet — not listed above.
