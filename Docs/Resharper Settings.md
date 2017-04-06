# Resharper Settings
Terminals uses [Resharper](http://www.jetbrains.com/resharper/) as a productivity tool for code style assistance and automatic refactoring. These option settings have been changed from the default Resharper settings for Terminals.

* **Code Inspection**
	* All
		* Redundancies in Symbol Declarations
			* Redundant field initializer: _Do not show_
		* Language Usage Opportunities
			* Use 'var' keyword when initializer explicitly declares type: _Do not show_
			* Use 'var' keyword when possible: _Do not show_
* **Langauges**
	* Common
		* Naming Style
			* Instance fields (private): _lowerCamelCase_ (instead of _lowerCamelCase)
		* Advanced settings
			* Event subscriptions on fields: _$object$_{"_"}_$event$_ (instead of $object$_On$event$) allows event handler methods named like: {"Button1_Click()"}
	* C#
		* Formatting Style
			* Braces Layout
				* Block under "case" label: _At next line (BSD style)_
				* Braces in "if-else" statement: _Use braces for mulitline_
				* Braces in "for" statement: _Add braces_
				* Braces in "foreach" statement: _Add braces_
				* Braces in "while" statement: _Add braces_
				* Braces in "using" statement: _Add braces_
				* Braces in "fixed" statement: _Add braces_
			* Blank Lines
				* Keep max blank lines in declarations: _1_
				* Keep max blank lines in code: _1_
			* Line Breaks and Wrapping
				* Break line in single embedded statement: _Break line_
				* Place "while on new line: _true_
				* Right margin (columns): _170_
				* Place simple accessor on single line: _false_
			* Spaces
				* Multiplicative operators (*,/,%): _true_
			* Other
				* Indent nested "using" statements: _true_
				* indent nested "fixed" statements: _true_
