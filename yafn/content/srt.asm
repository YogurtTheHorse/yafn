[section __code]
to_ind:
  swp
  push 0x4
  mul
  push arr
  add
  swp

  ret

fill_arr:
      push #n
      sav *i

      iter:
          push *i
          dpl
          dec
          call to_ind

          rnd

          sav

          dec
          dpl
          sav *i

          cmp0
          and2
          rsh2

          jnz iter

      ret

sort:
  push #n
  sav *cnt

  sort_iter:
    push 0x0
    sav *newn

    push 0x1

    second_lvl:
      ; getting indexes
      dpl
      dpl
      dec
      call to_ind
      swp
      call to_ind
      swp

      ; loading values
      smpsh
      swp
      smpsh
      swp
      
      ; doubling last two values
      sav *tmp
      dpl
      push *tmp
      swp
      push *tmp

      ; compairing
      cmp
      and2
      rsh2

      jez skip_swp
      ; swap and update newn

      sav *tmp
      sav *tmp2
      dpl
      sav *newn

      jmp saving
      
     skip_swp:
      sav *tmp2
      sav *tmp
      
    saving:
      ; getting indexes (again, but with another way)
      dpl
      dpl
      dec
      call to_ind
      swp
      call to_ind
      swp

      ; loading values
      push *tmp2
      sav
      push *tmp
      sav

      ; check end_loop
      inc
      dpl
      push *cnt
      
      eq
      jez second_lvl
      pop
    ; end second_lvl
      
    
    push *newn
    dpl
    sav *cnt

    push 0x2
    cmp

    and2
    rsh2

    jez sort_iter
  ; end sort
  ret

prnt:
  push 0x0
  
  another_loop:
    dpl
    call to_ind
    
    smpsh
    wrt

    inc
    dpl
    push #n
    dec
    eq
    jez another_loop
    pop
    
  ret

__start:
    call fill_arr
    brp
    call sort
    call prnt

[section __data]
i:    dd 0x0
j:    dd 0x0

swp:  dd 0x0
cnt:  dd 0x0
newn: dd 0x0

tmp:  dd 0x0
tmp2: dd 0x0
tmp3: dd 0x0

arr:  times 0x10 dd 0x0

[section __constants]
n:    dq 0x10
