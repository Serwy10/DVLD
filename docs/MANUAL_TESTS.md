# Windows verification

Use the clean demo database and synthetic records.

1. Log in with correct/incorrect credentials; deactivate a user and confirm login is denied. Enable username remembering, close/reopen, and then disable it. The password field must always be empty on reopening.
2. Open every management list before inserting any records. Empty lists must retain their columns.
3. Create/edit a person, keep the same national number, search by ID then national number, and upload/remove/change a photo. Confirm changing gender does not discard a selected photo.
4. Create/edit a user, leave the password blank during an edit, change the password through the main menu, sign out, and log in again. Test repeated sign-outs without nested windows.
5. Create a Class 3 application; schedule and record vision, written, and street tests in order. Confirm a result cannot be saved twice. View both pass and fail results.
6. Issue the first license and check the application becomes completed. Try issuing it again.
7. Replace the license as lost/damaged. Confirm the old license is inactive, the replacement keeps the original expiration date, and only the application fee is charged.
8. Issue an international license and try issuing another current license for the same driver. Try an expired, inactive, detained, and non-Class-3 license.
9. Detain/release a license, then detain it again. Confirm the current detention is shown, including its fine and release state.
10. Renew an expired active license. Confirm its new expiration follows the license-class validity length and the old license becomes inactive.
11. Search names containing an apostrophe, `%`, `[`, `]`, and `*`. Open a context menu with no selected row.
12. Disconnect SQL Server or provide a bad connection string. Confirm the application reports a useful error without exposing a password.
