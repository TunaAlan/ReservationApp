# Future Works

- Filter system by category and search button.
- Email integration: confirmation emails for registration and password change flows.
- Send reservation details to the customer's email after a reservation is completed.
- Passwordless OTP login for guest/one-time customers: email + randomly generated one-time code, no account required. Intended for customers who reserve infrequently (e.g. once or twice a year) and shouldn't need to manage a password.
- Loyalty perks for registered account holders (vs. OTP guests): early access windows to popular time slots before general availability, member-only discount percentage on Avg Price, one-click repeat of a past reservation, remembered seating/table preference, and more flexible cancellation policy.
- Identity-merge edge case to resolve before shipping dual auth: the same email can currently end up as both an OTP guest and a full account holder, splitting one customer's reservation history across two identities. Needs a resolution strategy (e.g. detect existing account on OTP login and prompt to sign in instead, or offer "upgrade this guest history to an account").
