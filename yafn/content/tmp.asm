[section __code]
	
	push 0x1
	neg
	sav *0x0
	brp

[section __data]
	resb 20

[section __constants]
n:
	db 0x10, 0x0, 0x0, 0x0
