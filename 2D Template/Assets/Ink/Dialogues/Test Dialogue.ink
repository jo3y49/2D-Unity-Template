// Basic Ink Dialogue Example
Hello, player!
-> start

=== start ===
+ Say hello
    Nice to meet you!
    -> response
+ Stay silent
    ...
    -> ignored

=== response ===
You too!
-> END

=== ignored ===
The conversation ends abruptly and rudely.
-> END
