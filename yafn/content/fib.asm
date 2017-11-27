[section __code]
iter:
	; checkinng loop condition
	push *0x0
	cmp0
	and2
	rsh2

	jez end

	; a, b = b + a, b
	swp
	dpl
	sav *0x5
	pop
	dpl
	push *0x5
	add

	push 0x1
	push *0x0

	sub
	sav *0x0

	jmp iter

__start:
  ; reading N from constants 
	; sub 1 from N for loop condition
	push 0x1
	push #0x0

	; write number to console
	dpl
	wrt

	sub

	sav *0x0

	push 0x0
	push 0x1

	; start loop
	jmp iter

end:
	; write top value from stack
	wrt

[section __data]
	resb 20

[section __constants]
n:
	db 0x10, 0x0, 0x0, 0x0
